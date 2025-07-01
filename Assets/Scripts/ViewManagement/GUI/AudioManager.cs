using UnityEngine;
using System.Collections;
using System.Threading;
using System;
using System.Drawing;
using Newtonsoft.Json.Linq;

/// <summary>
/// Can play one music and array of effects at same timeReward.
/// </summary>
[Prefab("AudioManager", true)]
public class AudioManager : Singleton<AudioManager> {

    #region Readonly fileds
    #endregion

    #region Serialize fields
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
	private AudioSource musicSource;
	[SerializeField]
	private AudioSource[] effectsSource;
    [SerializeField]
    private AudioSource[] effects3DSource;
    #endregion

    #region Private fields
    private AudioSource currentEffectSource;
    #endregion

    #region Properties
    public bool IsMusicMuted {
        get => gameManager.IsMusicMuted;
        set {
            gameManager.IsMusicMuted = value;

            MuteMusic(value);
        }
    }

    public bool IsEffectsMuted {
        get => gameManager.IsEffectsMuted;
        set {
            gameManager.IsEffectsMuted = value;

            MuteEffects(value);
        }
    }

    public float MusicVolume {
        get => gameManager.MusicVolume;
        set {
            gameManager.MusicVolume = value;

            SetMusicVolume(value);
        }
    }

    public float EffectsVolume {
        get => gameManager.EffectsVolume;
        set {
            gameManager.EffectsVolume = value;

            SetEffectsVolume(value);
        }
    }
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    public void LoadAudioStatus() {
        SetMusicVolume(gameManager.MusicVolume);
        SetEffectsVolume(gameManager.EffectsVolume);

        MuteMusic(gameManager.IsMusicMuted);
        MuteEffects(gameManager.IsEffectsMuted);
    }

    public void PlayMusic(AudioClip clip) {
        PlayAudioSource(musicSource, clip, true);
    }

	public void PlayMusic(AudioClip clip, bool loop) {
		PlayAudioSource(musicSource, clip, loop);
	}

	public void PlayEffect(AudioClip clip) {
        PlayEffect(clip, false);
    }

    public void PlayEffect(AudioClip clip, bool loop) {
        currentEffectSource = FindFreeAudioSource(effectsSource);

        if (currentEffectSource != null) {
            PlayAudioSource(currentEffectSource, clip, loop);
        }
    }

    public void Play3DEffect(AudioClip clip, Vector3 point, bool loop = false) {
        currentEffectSource = FindFreeAudioSource(effects3DSource);

        if (currentEffectSource != null) {
            currentEffectSource.transform.position = point;

            PlayAudioSource(currentEffectSource, clip, loop);
        }
    }

    public void StopMusic() {
		musicSource.Stop();
	}

	public void StopAllEffects() {
        foreach (var effectSource in effectsSource) {
            effectSource.Stop();
        }

        foreach (var effectSource in effects3DSource) {
            effectSource.Stop();
        }
    }

	
	#endregion

	#region Protected methods
	#endregion

	#region Private methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="clip"></param>
    /// <param name="loop"></param>
	private void PlayAudioSource(AudioSource audioSource, AudioClip clip, bool loop) {
		if (audioSource.isPlaying) {
			audioSource.Stop();
		}

        audioSource.loop = loop;
		audioSource.clip = clip;

		audioSource.Play();
        //audioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="audioSources"></param>
	private AudioSource FindFreeAudioSource(AudioSource[] audioSources) {
        return 
            Array.Find(audioSources, audioSource => !audioSource.isPlaying);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mute"></param>
    private void MuteMusic(bool mute) {
        musicSource.mute = mute;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mute"></param>
    private void MuteEffects(bool mute) {
        foreach (var effectSource in effectsSource) {
            effectSource.mute = mute;
        }

        foreach (var effectSource in effects3DSource) {
            effectSource.mute = mute;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="volume"></param>
    private void SetMusicVolume(float volume) {
        musicSource.volume = volume;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="volume"></param>
    private void SetEffectsVolume(float volume) {
        foreach (var effectSource in effectsSource) {
            effectSource.volume = volume;
        }

        foreach (var effectSource in effects3DSource) {
            effectSource.volume = volume;
        }
    }
    #endregion

    #region Coroutines
    #endregion
}