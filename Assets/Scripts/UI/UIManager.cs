using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PrizeData _prizeData;
    [SerializeField] private float _delayTime = 5;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _questionPanel;
    [SerializeField] private GameObject _buttonsPanel;
    [SerializeField] private GameObject _pizzaImage;
    [SerializeField] private GameObject _pizzaShadow;
    [SerializeField] private GameObject _cursePanel;
    [SerializeField] private Button _curseButton;
    [SerializeField] private GameObject _autoButton1;
    [SerializeField] private GameObject _autoButton2;
    [SerializeField] private List<Panel> _panels;

    public event Action PanelClosed;
    public event Action CurseClosed;

    private PrizeID[] _win = {  PrizeID.AutoClick, PrizeID.Curse, PrizeID.PlusOne, PrizeID.RandomPoints };
    private PrizeID[] _lose = { PrizeID.LOSE };
    private PrizeID [] _positive = { PrizeID.SpinCostBack, PrizeID.FreeSpin, PrizeID.IncreaseScore };
    private PrizeID [] _negative = { PrizeID.ReduceScore };

    private Panel _currentPanel;
    private IEnumerator _coroutine;

    private void Start()
    {
        foreach (Panel panel in _panels)
        {
            panel.button.onClick.AddListener(ClosePanels);
        }

        _curseButton.onClick.AddListener(HideCurse);
    }

    public Panel GetPanel(PanelID panelID)
    {
        Panel panel = _panels.Find(x => x.ID == panelID);

        return panel;
    }

    public void ShowPanel(PrizeID id)
    {
        if (_win.Contains(id))
        {
            _currentPanel = GetPanel(PanelID.WIN);
            ShowQuestion(false);
        }
        else if (_lose.Contains(id))
        {
            _currentPanel = GetPanel(PanelID.LOSE);
            ShowQuestion(false);
        }
        else if (_negative.Contains(id))
        {
            _currentPanel = GetPanel(PanelID.NEGATIVE);
        }
        else if (_positive.Contains(id))
        {
            _currentPanel = GetPanel(PanelID.POSITIVE);
        }
        _currentPanel.tmptext.text = _prizeData.GetText(id);
        _currentPanel.panel.SetActive(true);
        _coroutine = DelayedClosePanel();
        StartCoroutine(_coroutine);
    }

    private IEnumerator DelayedClosePanel()
    {
        yield return new WaitForSeconds(_delayTime);
        ClosePanels();
    }

    private void ClosePanels()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _gamePanel.SetActive(false);
        _pizzaImage.SetActive(true);
        _pizzaShadow.SetActive(true);

        _currentPanel.panel.SetActive(false);
        PanelClosed.Invoke();
    }

    public void ShowGamePanel()
    {
        _gamePanel.SetActive(true);
        ShowQuestion(true);
        _pizzaImage.SetActive(false);
        _pizzaShadow.SetActive(false);
    }

    private void ShowQuestion(bool show) {
        _questionPanel.SetActive(show);
        _buttonsPanel.SetActive(show);
    }
    public void ShowCurse()
    {
        _cursePanel.SetActive(true);
        _pizzaImage.SetActive(false);
        _pizzaShadow.SetActive(false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.cursed);
    }

    public void HideCurse()
    {
        _cursePanel.SetActive(false);
        _cursePanel.GetComponent<Image>().material.color = new Color(221f/255f, 78f/255f, 78f/255f);
        CurseClosed.Invoke();
    }

    public void ResetColor()
    {
        _cursePanel.GetComponent<Image>().material.color = Color.white;
    }
    public IEnumerator AutoClickAnimation(float duration, float interval)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.autoclick);
            _autoButton1.SetActive(true);
            yield return new WaitForSeconds(interval);

            _autoButton1.SetActive(false);
            _autoButton2.SetActive(true);

            yield return new WaitForSeconds(interval);
            _autoButton2.SetActive(false);

            timePassed += interval * 2;
        }
        _autoButton1.SetActive(false);
        _autoButton1.SetActive(false);
    }

    public void FormatText(float value)
    {
        string _formatted = string.Format(_currentPanel.tmptext.text, value);
        _currentPanel.tmptext.text = _formatted;
    }
}

public enum PanelID
{
    WIN,
    LOSE,
    POSITIVE,
    NEGATIVE
}

[System.Serializable]
public class Panel
{
    public PanelID ID;
    public GameObject panel;
    public TMP_Text tmptext;
    public Button button;
}
