using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Pool;
using System.Collections;
using Zenject;

public class PointsTextPool : MonoBehaviour
{
    [SerializeField] private TMP_Text _textPrefab;
    [SerializeField] private RectTransform _spawnArea;
    [SerializeField] private int _maxSize = 20;
    [SerializeField] private float _timeToReturn = 1f;

    private ClickHandler _clickHandler;
    private ScoreManager _scoreManager;
    private TMP_Text [] _points;
    private int index = 0;

    [Inject]
    private void Construct(ClickHandler clickHandler, ScoreManager scoreManager)
    {
        _clickHandler = clickHandler;
        _scoreManager = scoreManager;
    }

    private void OnEnable()
    {
        _clickHandler.OnClick += SpawnPointText;
    }
    private void OnDisable() {
        _clickHandler.OnClick -= SpawnPointText;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _points = new TMP_Text[_maxSize];
        for (int i = 0; i < _maxSize; i++)
        {
            TMP_Text pointText = Instantiate(_textPrefab, _spawnArea);
            pointText.gameObject.SetActive(false);
            _points[i] = pointText;
        }
    }

    private void SpawnPointText()
    {
        TMP_Text pointText = _points[index];
        pointText.gameObject.SetActive(true);
        pointText.text = "+" + _scoreManager.ClickMultiplier.ToString();
        Vector2 randomPosition = new Vector2(
                Random.Range(_spawnArea.rect.min.x, _spawnArea.rect.max.x),
                Random.Range(_spawnArea.rect.min.y, _spawnArea.rect.max.y)
            );
        pointText.rectTransform.anchoredPosition = new Vector2(randomPosition.x, randomPosition.y);

        StartCoroutine(ReturnToPool(pointText, index));
        index++;
        if (index >= _maxSize)
        {
            index = 0;
        }
    }

    IEnumerator ReturnToPool(TMP_Text pointText, int index)
    {
        yield return new WaitForSeconds(_timeToReturn);
        pointText.gameObject.SetActive(false);
    }
}
