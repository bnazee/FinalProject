using UnityEngine;
using CandyCoded.HapticFeedback;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource spinSource;

    public AudioClip autoclick;
    public AudioClip spinButton;
    public AudioClip button;
    public AudioClip clickButton;
    public AudioClip dropTopping;
    public AudioClip toppingDropped1;
    public AudioClip toppingDropped2;
    public AudioClip wrongAnswer;
    public AudioClip prizeAutoClick;
    public AudioClip prizeFreeSpin;
    public AudioClip prizePoints;
    public AudioClip prizeRollbackSpin;
    public AudioClip prizeReduceScore;
    public AudioClip prizePlusOne;
    public AudioClip cursed;
    public AudioClip curseEnded;
    public AudioClip rofl;

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

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void PlaySpinning()
    {
        spinSource.Play();
    }

    public void StopSpinning()
    {
        spinSource.Stop();
    }
    public void PlayRandomDrop()
    {
        int r = Random.Range(0, 2);
        if (r == 0)
        {
            sfxSource.PlayOneShot(toppingDropped1);
        }
        else if (r == 1)
        {
            sfxSource.PlayOneShot(toppingDropped2);
        }
    }

    public void PlayMainClick()
    {
        sfxSource.PlayOneShot(clickButton);
        if (VibrationSettings.Instance.VibrationOn)
        {
            HapticFeedback.HeavyFeedback();
        }
    }
    public void PlayClick()
    {
        sfxSource.PlayOneShot(button);
        if (VibrationSettings.Instance.VibrationOn)
        {
            HapticFeedback.MediumFeedback();
        }
    }

    public void PlaySpinButton()
    {
        sfxSource.PlayOneShot(spinButton);
        if (VibrationSettings.Instance.VibrationOn)
        {
            HapticFeedback.HeavyFeedback();
        }
    }
}
