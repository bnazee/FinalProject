using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Image _muteMusicImage;
    [SerializeField] private Image _muteSFXImage;
    [SerializeField] private Button _muteMusicButton; 
    [SerializeField] private Button _muteSFXButton;

    private static readonly string MusicVolumeKey = "MusicVolume";
    private static readonly string SfxVolumeKey = "SFXVolume";
    private static readonly float MinValue = 0.0001f;

    void Start()
    {
        LoadVolume();
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        _muteMusicButton.onClick.AddListener(MuteMusic); 
        _muteSFXButton.onClick.AddListener(MuteSFX);
        UpdateVolume();
    }

    void LoadVolume()
    {
        _musicSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        _sfxSlider.value = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
    }

    void SaveVolume()
    {
        PlayerPrefs.SetFloat(MusicVolumeKey, _musicSlider.value);
        PlayerPrefs.SetFloat(SfxVolumeKey, _sfxSlider.value);
        PlayerPrefs.Save();
    }

    void UpdateVolume()
    {
        _audioMixer.SetFloat(MusicVolumeKey, Mathf.Log10(_musicSlider.value) * 20);
        _audioMixer.SetFloat(SfxVolumeKey, Mathf.Log10(_sfxSlider.value) * 20);

        _muteMusicImage.gameObject.SetActive(Mathf.Approximately(_musicSlider.value, MinValue));
        _muteSFXImage.gameObject.SetActive(Mathf.Approximately(_sfxSlider.value, MinValue));
    }

    public void SetMusicVolume(float sliderValue)
    {
        SaveVolume();
        UpdateVolume();
    }

    public void SetSFXVolume(float sliderValue)
    {
        SaveVolume();
        UpdateVolume();
    }

    public void MuteMusic()
    {
        if (Mathf.Approximately(_musicSlider.value, MinValue))
        {
            _musicSlider.value = 1f;
        }
        else
        {
            _musicSlider.value = MinValue;
        }
    }

    public void MuteSFX()
    {
        if (Mathf.Approximately(_sfxSlider.value, MinValue))
        {
            _sfxSlider.value = 1f;
        }
        else
        {
            _sfxSlider.value = MinValue;
        }
    }
}


