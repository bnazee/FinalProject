using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterInputField : MonoBehaviour
{
    [SerializeField] private Button okButton;
    private TMP_InputField inputField;
    public event Action<string> OkAction;
    [SerializeField] GameObject[] _lettersAndButtons;

    void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener(EnforceSingleLetter);
        okButton.onClick.AddListener(SendLetter);
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

    private void EnforceSingleLetter(string input)
    {
        inputField.text = input.ToUpper();
        if (input.Length > 1)
        {
            inputField.text = input.Substring(0, 1);
        }
    }

    private void SendLetter()
    {
        string word = inputField.text.ToUpper().Replace(" ", "");
        if (word.Length > 0)
        {
            OkAction.Invoke(word);
        }
        transform.parent.gameObject.SetActive(false);
    }
}
