using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PrizeManager : MonoBehaviour
{
    [SerializeField] private float _decreasePercentage;
    [SerializeField] private PrizeData _prizeData;

    private Dictionary<int, System.Action> _prizeActions;
    private int _prizeIndex;
    private PrizeID _prizeID;
    private IEnumerator _curse;
    private bool _isWon = true;

    private QuestionsManager _questionsManager;
    private WheelRotator _wheelRotator;
    private ScoreManager _scoreManager;
    private SpinController _spinController;
    private UIManager _uiManager;
    private ToppingManager _toppingManager;
    private AudioManager _audioManager;

    [Inject]
    private void Construct(WheelRotator wheelRotator, ScoreManager scoreManager,
        QuestionsManager questionsManager, SpinController spinController,
        UIManager uiManager, ToppingManager toppingManager)
    {
        _wheelRotator = wheelRotator;
        _scoreManager = scoreManager;
        _questionsManager = questionsManager;
        _spinController = spinController;
        _uiManager = uiManager;
        _toppingManager = toppingManager;
        InitializePrizeActions();
    }
    private void OnEnable()
    {
        _wheelRotator.GotPrize += PrizeChecker;
        _uiManager.PanelClosed += DropTopping;
        _uiManager.CurseClosed += Curse;
    }

    private void OnDisable()
    {
        _wheelRotator.GotPrize -= PrizeChecker;
        _uiManager.PanelClosed -= DropTopping;
        _uiManager.CurseClosed -= Curse;
    }

    private void Start()
    {
        _audioManager = AudioManager.Instance;
    }

    private void InitializePrizeActions()
    {
        _prizeActions = new Dictionary<int, System.Action>
        {
            { _prizeData.GetIndex(PrizeID.SpinCostBack), RollSpinCostBack },
            { _prizeData.GetIndex(PrizeID.PlusOne), StartGame },
            { _prizeData.GetIndex(PrizeID.ReduceScore), ReduceScore },
            { _prizeData.GetIndex(PrizeID.FreeSpin), FreeSpin },
            { _prizeData.GetIndex(PrizeID.IncreaseScore), IncreaseScore },
            { _prizeData.GetIndex(PrizeID.Curse), () => _uiManager.ShowCurse() },
            { _prizeData.GetIndex(PrizeID.AutoClick), StartGame },
            { _prizeData.GetIndex(PrizeID.RandomPoints), StartGame },
            { _prizeData.GetIndex(PrizeID.ROFL), Rofl }
        };
    }

    private void PrizeChecker(int prizeIndex)
    {
        _prizeIndex = prizeIndex;
        _prizeID = _prizeData.GetID(prizeIndex);
        if (_prizeActions.TryGetValue(prizeIndex, out var action))
        {
            action.Invoke();          
        }
    }

    private void StartGame()
    {
        _uiManager.ShowGamePanel();
        StartCoroutine(_questionsManager.StartGame(isWon => OnGameEnd(isWon)));
    }

    private void OnGameEnd(bool isWon)
    {
        _isWon = isWon;
        if (isWon)
        {
            if (_prizeID == PrizeID.PlusOne)
            {
                PlusOne();
            }
            else if (_prizeID == PrizeID.Curse)
            {
                CurseEnded();
            }
            else if (_prizeID == PrizeID.AutoClick)
            {
                StartCoroutine(AutoClick(30f, 0.1f));
            }
            else if (_prizeID == PrizeID.RandomPoints)
            {
                AddRandomPoints();
            }

        }
        else
        {
            _uiManager.ShowPanel(PrizeID.LOSE);
            _audioManager.PlaySFX(_audioManager.wrongAnswer);
        }
    }

    private void PlusOne()
    {
        _scoreManager.IncreaseMultiplier();
        _audioManager.PlaySFX(_audioManager.prizePlusOne);
        _uiManager.ShowPanel(_prizeID);
    }

    private void RollSpinCostBack()
    {
        _audioManager.PlaySFX(_audioManager.prizeRollbackSpin);
        int rollBack = 0;
        for (int i = 0; i < 5; i++)
        {
            if (_spinController.SpinCost - _spinController.SpinCostRase >= 0)
            {
                _spinController.SpinCost -= _spinController.SpinCostRase;
                Debug.Log(_spinController.SpinCost);
                rollBack++;
            }
        }
        rollBack = Mathf.Max(1, rollBack - 1);
        _spinController.UpdateSpinCost();
        _uiManager.ShowPanel(_prizeID);
        _uiManager.FormatText(rollBack);
    }

    private void ReduceScore()
    {
        _audioManager.PlaySFX(_audioManager.prizeReduceScore);
        float reducePercentage = Random.Range(5, 11) / 100f;
        int newScore = (int)(_scoreManager.Score * (1 - reducePercentage));
        newScore = Mathf.Max(1, newScore);
        _scoreManager.UpdateScore(newScore);
        _uiManager.ShowPanel(_prizeID);
        _uiManager.FormatText(reducePercentage * 100);
    }

    private void FreeSpin()
    {
        _audioManager.PlaySFX(_audioManager.prizeFreeSpin);
        _spinController.ActivateFreeSpin();
        _uiManager.ShowPanel(_prizeID);
    }

    private void IncreaseScore()
    {
        _audioManager.PlaySFX(_audioManager.prizePoints);
        float increasePercentage = Random.Range(5, 11) / 100f;
        int newScore = (int)(_scoreManager.Score * (1 + increasePercentage));
        _scoreManager.UpdateScore(newScore);
        _uiManager.ShowPanel(_prizeID);
        _uiManager.FormatText(increasePercentage * 100);
    }

    private void AddRandomPoints()
    {
        _audioManager.PlaySFX(_audioManager.prizePoints);
        int randomPoints = Random.Range(_spinController.SpinCost, _spinController.SpinCost * 5);
        int newScore = _scoreManager.Score + randomPoints;
        _scoreManager.UpdateScore(newScore);
        _uiManager.ShowPanel(_prizeID);
        _uiManager.FormatText(randomPoints);
    }

    private void Curse()
    {
        StartGame();
        _questionsManager.SetCursed();
        _curse = DecreaseScoreOverTime();
        StartCoroutine(_curse);
    }

    private void CurseEnded()
    {
        StopCoroutine(_curse);
        _uiManager.ResetColor();
        _uiManager.ShowPanel(_prizeID);
        _audioManager.PlaySFX(_audioManager.curseEnded);
    }

    private void Rofl()
    {
        DropTopping();
        _audioManager.PlaySFX(_audioManager.rofl);
    }

    private IEnumerator DecreaseScoreOverTime()
    {
        while (_scoreManager.Score > 0)
        {
            int decreaseAmount = Mathf.Max(1, (int)(_scoreManager.Score * (_decreasePercentage / 100f)));
            int newScore = _scoreManager.Score - decreaseAmount;
            newScore = Mathf.Max(0, newScore);
            _scoreManager.UpdateScore(newScore);
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator AutoClick(float duration, float interval)
    {
        _audioManager.PlaySFX(_audioManager.prizeAutoClick);
        _uiManager.ShowPanel(_prizeID);
        StartCoroutine(_uiManager.AutoClickAnimation(duration, interval));
        float timePassed = 0f;
        while (timePassed < duration)
        {
            yield return new WaitForSeconds(interval);

            int newScore = _scoreManager.Score + _scoreManager.ClickMultiplier;
            _scoreManager.UpdateScore(newScore);

            yield return new WaitForSeconds(interval);

            newScore = _scoreManager.Score + _scoreManager.ClickMultiplier;
            _scoreManager.UpdateScore(newScore);

            timePassed += interval * 2;
        }
    }

    private void DropTopping()
    {
        if (_isWon)
        {
            _toppingManager.DropTopping(_prizeIndex);
            _audioManager.PlaySFX(_audioManager.dropTopping);
        }
        else
        {
            _spinController.IsPlaying = false;
        }
        _isWon = true;
    }
}
