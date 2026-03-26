using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("UI Sounds")]
    public AudioSource uiAudioSource;           // Panel open/close & button click
    public AudioClip panelOpenClip;
    public AudioClip panelCloseClip;
    public AudioClip buttonClickClip;
    public AudioClip buttonHoverClip;
    public AudioClip buttonDisabledClip;      
    public AudioClip errorClip; // Short error beep
    public AudioClip fartNoiseClip;

    [Header("Gameplay Sounds")]
    public AudioSource gameplayAudioSource;     // Looping audio while dialogue types
    public AudioClip enokiSpeakingClip1;
    public AudioClip enokiSpeakingClip2;
    public AudioClip enokiSpeakingClip3;
    public AudioClip walkingClip;  // Looping walking sound

    [Header("MiniGame Sounds")]
    public AudioSource miniGame1AudioSource;     
    public AudioClip fountainClip; // Looping fountain sound
    public AudioClip keyActivateClip;
    public AudioClip keyDepositClip;


    void Awake()
    {
        // Singleton pattern
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

    // UI methods
    public void PlayPanelOpen()
    {
        if (uiAudioSource != null && panelOpenClip != null)
            uiAudioSource.PlayOneShot(panelOpenClip);
    }

    public void PlayPanelClose()
    {
        if (uiAudioSource != null && panelCloseClip != null)
            uiAudioSource.PlayOneShot(panelCloseClip);
    }

    public void PlayButtonClick()
    {
        if (uiAudioSource != null && buttonClickClip != null)
            uiAudioSource.PlayOneShot(buttonClickClip);
    }

    public void PlayButtonHover()
    {
        if (uiAudioSource != null && buttonHoverClip != null)
            uiAudioSource.PlayOneShot(buttonHoverClip);
    }

    public void PlayButtonDisabled()
    {
        if (uiAudioSource != null && buttonDisabledClip != null)
            uiAudioSource.PlayOneShot(buttonDisabledClip);
    }

    // Dialogue speaking
    public void StartSpeakingEnoki1()
    {
        if (gameplayAudioSource != null && enokiSpeakingClip1 != null && !gameplayAudioSource.isPlaying)
        {
            gameplayAudioSource.clip = enokiSpeakingClip1;
            gameplayAudioSource.loop = true;
            gameplayAudioSource.Play();
        }
    }

    public void StartSpeakingEnoki2()
    {
        if (gameplayAudioSource != null && enokiSpeakingClip2 != null && !gameplayAudioSource.isPlaying)
        {
            gameplayAudioSource.clip = enokiSpeakingClip2;
            gameplayAudioSource.loop = true;
            gameplayAudioSource.Play();
        }
    }

    public void StartSpeakingEnoki3()
    {
        if (gameplayAudioSource != null && enokiSpeakingClip3 != null && !gameplayAudioSource.isPlaying)
        {
            gameplayAudioSource.clip = enokiSpeakingClip3;
            gameplayAudioSource.loop = true;
            gameplayAudioSource.Play();
        }
    }

    public void StopSpeaking()
    {
        if (gameplayAudioSource != null && gameplayAudioSource.isPlaying)
            gameplayAudioSource.Stop();
    }

    // Player walking
    public void StartWalking()
    {
        if (gameplayAudioSource != null && walkingClip != null && !gameplayAudioSource.isPlaying)
        {
            gameplayAudioSource.clip = walkingClip;
            gameplayAudioSource.loop = true;
            gameplayAudioSource.Play();
        }
    }

    public void StopWalking()
    {
        if (gameplayAudioSource != null && gameplayAudioSource.isPlaying)
            gameplayAudioSource.Stop();
    }

    // Fountain
    public void StartFountain()
    {
        if (miniGame1AudioSource != null && fountainClip != null && !miniGame1AudioSource.isPlaying)
        {
            miniGame1AudioSource.clip = fountainClip;
            miniGame1AudioSource.loop = true;
            miniGame1AudioSource.Play();
        }
    }

    public void StopFountain()
    {
        if (miniGame1AudioSource != null && miniGame1AudioSource.isPlaying)
            miniGame1AudioSource.Stop();
    }

    // Error / feedback
    public void PlayError()
    {
        if (uiAudioSource != null && errorClip != null)
            uiAudioSource.PlayOneShot(errorClip);
    }

    public void KeyActivated ()
    {
        if (miniGame1AudioSource != null && keyActivateClip != null)
            miniGame1AudioSource.PlayOneShot(keyActivateClip);
    }
    public void KeyDestroyed ()
    {
        if (miniGame1AudioSource != null && keyDepositClip != null)
            miniGame1AudioSource.PlayOneShot(keyDepositClip);
    }

    public void PlayFartNoise()
    {
        if (uiAudioSource != null && fartNoiseClip != null)
            uiAudioSource.PlayOneShot(fartNoiseClip);
    }
}