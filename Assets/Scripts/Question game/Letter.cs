using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    [SerializeField] private TMP_Text letterText;
    
    public void SetLetter(string letter)
    {
        letterText.text = letter;
    }
    
}
