using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class QuestionsManager : MonoBehaviour
{
    [SerializeField] private List<Letter> _letterObjects;
    [SerializeField] private TMP_Text _questionText;
    [SerializeField] private TMP_Text _tryText;
    [SerializeField] private int _possibleTryCount = 4;

    private string _answer;
    private string _guessedLetters;
    private int _tryCount = 0;
    private bool _isWon;
    private bool _isCursed;
    private Question[] _questionArray = new Question[0];
    private List<int> _answeredList = new List<int>();

    private LetterInputField _letterInputField;
    private WordInputField _wordInputField;
    private SaverLoader _saverLoader;

    public delegate void GameEndCallback(bool isWon);

    [Inject]
    public void Construct(LetterInputField letterInputField, WordInputField wordInputField, SaverLoader saverLoader)
    {
        _letterInputField = letterInputField;
        _wordInputField = wordInputField;
        _saverLoader = saverLoader;
    }

    private void OnEnable()
    {
        _letterInputField.OkAction += OnLetterRecieved;
        _wordInputField.OkAction += CheckAnswer;
        _saverLoader.onJsonLoadedCallback += OnJsonLoaded;
    }

    private void OnDisable()
    {
        _letterInputField.OkAction -= OnLetterRecieved;
        _wordInputField.OkAction -= CheckAnswer;
        _saverLoader.onJsonLoadedCallback -= OnJsonLoaded;
    }

    private void Start()
    {
        _answeredList = _saverLoader.LoadAnswered();     
    }
    private void OnJsonLoaded()
    {
        _questionArray = _saverLoader.LoadQuestions();
    }
    public IEnumerator StartGame(GameEndCallback onGameEnd)
    {
        DeactivateLetters();
        SetRandomQuestion();
        _guessedLetters = new string('_', _answer.Length);
        _tryCount = 0;
        UpdateTryText();
        _isWon = false;
        _isCursed = false;

        while (!_isWon && (_isCursed || _tryCount < _possibleTryCount))
        {
            yield return null;
        }

        ShowAnswer();
        onGameEnd(_isWon);
    }

    private void SetRandomQuestion()
    {
        int randomQuestion = 0;
        if (_answeredList.Count >= _questionArray.Length)
        {
            Debug.Log("Вопросы исчерпались");
            _answeredList = new List<int>();
        }
        do
        {
            randomQuestion = Random.Range(0, _questionArray.Length);
        }
        while (_answeredList.Contains(randomQuestion));

        _answeredList.Add(randomQuestion);
        _saverLoader.SaveAnswered(_answeredList);

        _questionText.text = _questionArray[randomQuestion].question;
        _answer = _questionArray[randomQuestion].answer.ToUpper();
        ActivateLetters(_answer);
    }

    private void ActivateLetters(string word)
    {
        for (int i = 0; i < word.Length; i++)
        {
            _letterObjects[i].gameObject.SetActive(true);
            _letterObjects[i].SetLetter(string.Empty);
        }
    }

    private void DeactivateLetters()
    {
        foreach (Letter letter in _letterObjects)
        {
            letter.gameObject.SetActive(false);
        }
    }

    private void OnLetterRecieved(string letter)
    {
        for (int i = 0; i < _answer.Length; i++)
        {
            if (letter.Equals(_answer[i].ToString()))
            {
                _letterObjects[i].SetLetter(letter.ToString());
                _guessedLetters = _guessedLetters.Remove(i, 1).Insert(i, letter.ToString());
            }
        }

        CheckAnswer(_guessedLetters);
    }

    private void CheckAnswer(string answer)
    {
        if (answer.Equals(_answer))
        {
            _isWon = true;
        }
        if (!_isCursed)
        {
            _tryCount++;
            UpdateTryText();
        }
    }

    private void ShowAnswer()
    {
        for (int i = 0; i < _answer.Length; i++)
        {
            _letterObjects[i].SetLetter(_answer[i].ToString());         
        }
    }

    private void UpdateTryText()
    {
        _tryText.text = $"Попыток: {_possibleTryCount - _tryCount}";
    }

    public void SetCursed()
    {
        _isCursed = true;
        _tryText.text = _isCursed ? "Бесконечно" : $"Попыток: {_possibleTryCount - _tryCount}";
    }
}

