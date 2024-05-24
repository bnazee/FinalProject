using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalibrateButton : MonoBehaviour
{
    public Button calibrate;
    public GameObject panel;
    public Button score;
    public Button spincost;
    public Button spincostRaise;
    public Button multi;
    public TMP_InputField input;
    public ScoreManager scoreManager;
    public SpinController spinController;

    private void Start()
    {
        calibrate.onClick.AddListener(() =>
        {
            panel.SetActive(true);
            input.text = string.Empty;
        });
        score.onClick.AddListener(() =>
        {
            if (input.text.Length > 0)
            {
                scoreManager.UpdateScore(int.Parse(input.text));
            }
            panel.SetActive(false);
        });
        spincost.onClick.AddListener(() =>
        {
            if (input.text.Length > 0)
            {
                spinController.SpinCost = int.Parse(input.text);
                spinController.UpdateSpinCost();
            }
            panel.SetActive(false);
        });
        spincostRaise.onClick.AddListener(() =>
        {
            if (input.text.Length > 0)
            {
                spinController.SpinCostRase = int.Parse(input.text);
            }
            panel.SetActive(false);
        });
        multi.onClick.AddListener(() =>
        {
            if (input.text.Length > 0)
            {
                scoreManager.ClickMultiplier = int.Parse(input.text);
            }
            panel.SetActive(false);
        });

    }
}
