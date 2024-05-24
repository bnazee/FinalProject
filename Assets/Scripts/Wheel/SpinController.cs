using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SpinController : MonoBehaviour
{
    [SerializeField] private TMP_Text _spinCostText;
    [SerializeField] private Button _spinButton;

    [SerializeField] private int _spinCostRase;

    private int _spinCost;
    public event Action OnSpin;
    private bool _hasFreeSpin = false;
    private int _previousSpinCost;
    private ScoreManager _scoreManager;
    private SaverLoader _saverLoader;
    private bool _isPlaying = false;

    public int SpinCost { get => _spinCost; set => _spinCost = value; }
    public int SpinCostRase { get => _spinCostRase; set => _spinCostRase = value; }
    public bool IsPlaying { get => _isPlaying; set => _isPlaying = value; }

    [Inject]
    private void Construct(ScoreManager scoreManager, SaverLoader saverLoader)
    {
        _scoreManager = scoreManager;
        _saverLoader = saverLoader;
    }

    private void Start()
    {
        _spinButton.onClick.AddListener(TrySpin);
        _spinCost = _saverLoader.LoadSpinCost();
        UpdateSpinCost();
    }
    private void Update()
    {
        if (_isPlaying || (_scoreManager.Score < _spinCost))
        {
            _spinButton.interactable = false;
        }
        else
        {
            _spinButton.interactable = true;
        }
    }

    private void TrySpin()
    {
        if (_hasFreeSpin)
        {
            UseFreeSpin();
            return;
        }
        else if (_scoreManager.Score >= _spinCost)
        {
            _scoreManager.UpdateScore(_scoreManager.Score - _spinCost);
            _spinCost += _spinCostRase;
            UpdateSpinCost();
            OnSpin.Invoke();
            _isPlaying = true;
        }
    }

    private void UseFreeSpin()
    {
        _hasFreeSpin = false;
        OnSpin.Invoke();
        _isPlaying = true;
        _spinCost = _previousSpinCost;
        UpdateSpinCost();
    }

    public void UpdateSpinCost()
    {
        _spinCostText.text = _spinCost.ToString();
        _saverLoader.SaveSpinCost(_spinCost);
    }

    public void ActivateFreeSpin()
    {
        _hasFreeSpin = true;
        _previousSpinCost = _spinCost;
        _spinCost = 0;
        UpdateSpinCost();
    }  
}

