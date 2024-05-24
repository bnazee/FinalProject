using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordInputField : MonoBehaviour
{
    [SerializeField] private Button okButton;
    private TMP_InputField inputField;
    public event Action<string> OkAction;
    [SerializeField] GameObject[] _lettersAndButtons;


    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        okButton.onClick.AddListener(SendWord);
    }

    private void OnEnable()
    {
        inputField.Select();
        inputField.text = "";
        foreach (var obj in _lettersAndButtons)
        {
            obj.SetActive(false);
        }
    }

    private void OnDisable()
    {
        foreach (var obj in _lettersAndButtons)
        {
            obj.SetActive(true);
        }
    }

    private void SendWord()
    {
        string word = inputField.text.ToUpper().Replace(" ", "");
        if (word.Length > 0)
        {
            OkAction.Invoke(word);
        }
        transform.parent.gameObject.SetActive(false);
    }
}
