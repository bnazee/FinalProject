using System;
using TMPro;
using UnityEngine;
using Zenject;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private TMP_Text _scoreText;

    private ClickHandler _clickHandler;
    private SaverLoader _saverLoader;

    private int _score;
    private int _clickMultiplier;

    public int Score { get => _score; }
    public int ClickMultiplier { get => _clickMultiplier; set => _clickMultiplier = value; }

    [Inject]
    private void Construct(ClickHandler clickHandler, SaverLoader saverLoader)
    {
        _clickHandler = clickHandler;
        _saverLoader = saverLoader;
    }

    private void OnEnable()
    {

        _clickHandler.OnClick += OnClick;
    }
    private void OnDisable()
    {
        _clickHandler.OnClick -= OnClick;
    }

    private void Start()
    {
        _score = _saverLoader.LoadScore();
        _clickMultiplier = _saverLoader.LoadMultiplier();
        UpdateScore(_score);
    }

    private void OnClick()
    {
        int newScore = _score + _clickMultiplier;
        UpdateScore(newScore);
        _particleSystem.Play();
    }
    public void UpdateScore(int score)
    {
        _score = score;
        _scoreText.text = _score.ToString();
        _saverLoader.SaveScore(score);
    }

    public void IncreaseMultiplier()
    {
        _clickMultiplier++;
        _saverLoader.SaveMultiplier(_clickMultiplier);
    }
}
