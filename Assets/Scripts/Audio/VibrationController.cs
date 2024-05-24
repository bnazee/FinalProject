using UnityEngine;
using UnityEngine.UI;

public class VibrationSettings : MonoBehaviour
{
    public static VibrationSettings Instance { get; private set; }

    [SerializeField] private Button _vibrationButton;
    [SerializeField] private Image _vibrationMuteImage;

    public bool VibrationOn = true;

    private string VibrationKey = "Vibration";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadVibrationSettings();
        _vibrationButton.onClick.AddListener(ToggleVibration);
        UpdateVibrationImage();
    }

    void LoadVibrationSettings()
    {
        VibrationOn = PlayerPrefs.GetInt(VibrationKey, 1) == 1;
    }

    void SaveVibrationSettings()
    {
        PlayerPrefs.SetInt(VibrationKey, VibrationOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleVibration()
    {
        VibrationOn = !VibrationOn;
        SaveVibrationSettings();
        UpdateVibrationImage();
    }

    void UpdateVibrationImage()
    {
        _vibrationMuteImage.gameObject.SetActive(!VibrationOn);
    }
}
