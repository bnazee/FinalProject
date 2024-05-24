using UnityEngine;
using UnityEngine.UI;

public class OtherButtons : MonoBehaviour
{
    [SerializeField] private Button _letterButton;
    [SerializeField] private GameObject _letterPanel;
    [SerializeField] private Button _wordButton;
    [SerializeField] private GameObject _wordPanel;
    [SerializeField] private GameObject _rulesPanel;
    [SerializeField] private Button _openRules;
    [SerializeField] private Button _closeRules;
    [SerializeField] private GameObject _soundPanel;
    [SerializeField] private Button _openSound;
    [SerializeField] private Button _closeSound;
    void Start()
    {
        _letterButton.onClick.AddListener(() => {
            _letterPanel.SetActive(true);
            
        });      

        _wordButton.onClick.AddListener(() => {
            _wordPanel.SetActive(true);
        });

        _openRules.onClick.AddListener(() => {
            _rulesPanel.SetActive(true);
        });

        _closeRules.onClick.AddListener(() => {
            _rulesPanel.SetActive(false);
        });

        _openSound.onClick.AddListener(() => {
            _soundPanel.SetActive(true);
        });

        _closeSound.onClick.AddListener(() => {
            _soundPanel.SetActive(false);
        });
    }
}
