using DigitalRuby.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainAudio : MonoBehaviour
{
    private static MainAudio instance = null;

    public AudioSource sourceMusic = null;
    public AudioSource sourceEffects = null;

    private AudioClip defaultClip = null;

    [Range(0.0f, 1.0f)]
    public float sfVolume = 1.0f;
    public float FadeDelay = 1;

    static Coroutine FadingIn = null;
    static Coroutine FadingOut = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            defaultClip = sourceMusic.clip;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            sourceMusic.Stop();
            Destroy(gameObject);
        }
    }

    ///// <summary>
    ///// Plays Music or sound effects immidiately
    ///// </summary>
    ///// <param name="audio"></param>
    ///// <param name="asSoundEffect"></param>
    ///// <param name="volume"></param>
    //public static void Play(AudioClip audio)
    //{
    //    instance.StartCoroutine(instance.PlayMusic(audio));
    //}

    public static void PlaySoundEffect(AudioClip audio, float volume = 0.0f)
    {
        instance.playSoundEffect(audio, volume);
    }

    public static bool IsPlayingClip(AudioClip backgroundMusic)
    {
        return instance.PlayingClip(backgroundMusic);
    }

    #region Muting
    private static bool? isAudioOn = null;
    public static bool IsMuted()
    {
        if (!isAudioOn.HasValue)
        {
            if(!PlayerPrefs.HasKey(LS.Common.TermStrings.AllAudioOn))
            {
                PlayerPrefs.SetInt(LS.Common.TermStrings.AllAudioOn, 1);
                isAudioOn = true;
                PlayerPrefs.Save();
            }
            else
            {
                isAudioOn = PlayerPrefs.GetInt(LS.Common.TermStrings.AllAudioOn, 1) == 1;
            }
        }

        return !isAudioOn.Value;
    }
    public static void ToggleMuted(bool ToMute)
    {
        isAudioOn = !ToMute;
        PlayerPrefs.SetInt(LS.Common.TermStrings.AllAudioOn, isAudioOn.Value ? 1 : 0);
        PlayerPrefs.Save();

        instance.TogglePauseMusic(!isAudioOn.Value);
    }
    #endregion

    /// <summary>
    /// Plays new Music clip by fading from one to another.
    /// </summary>
    /// <param name="clip">New Music to play</param>
    public static void PlayWithFade(AudioClip clip)
    {
        instance.StartCoroutine(instance.FadeToMusic(clip));
    }

    public static void FadeMusicOut()
    {
        if (FadingOut == null)
        {
            if (FadingIn != null)
            {
                instance.StopCoroutine(FadingIn);
                FadingIn = null;
            }

            FadingOut = instance.StartCoroutine(instance.FadeOut());
        }
    }

    public static void FadeMusicIn()
    {
        if (FadingIn == null)
        {
            if (FadingOut != null)
            {
                instance.StopCoroutine(FadingOut);
                FadingOut = null;
            }

            FadingIn = instance.StartCoroutine(instance.FadeIn());
        }
    }


    public bool PlayingClip(AudioClip clip)
    {
        return sourceMusic.clip == clip && sourceMusic.isPlaying;
    }

    public void TogglePauseMusic(bool isPaused)
    {
        if (sourceMusic.clip != null)
        {
            if (isPaused && sourceMusic.isPlaying)
            {
                sourceMusic.Pause();
            }
            else if (!isPaused)
            {
                sourceMusic.UnPause();
            }
        }
    }


    /// <summary>
    /// Plays audio one shot for sond effects
    /// </summary>
    /// <param name="audio"></param>
    /// <param name="volume"></param>
    private void playSoundEffect(AudioClip audio, float volume = 0.0f)
    {
        if (!IsMuted())
        {
            sourceEffects.PlayOneShot(audio, (volume <= 0.01f ? instance.sfVolume : volume));
        }
    }

    public void PlaySoundEffectInstant(AudioClip audio)
    {
        instance.playSoundEffect(audio, instance.sfVolume);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (!IsMuted())
        {
            instance.sourceMusic.Stop();
            instance.sourceMusic.clip = clip;
            instance.sourceMusic.Play();
        }
    }

    private IEnumerator FadeToMusic(AudioClip clip)
    {
        if (!IsMuted() && instance.sourceMusic.clip != clip)
        {
            yield return FadeOut();
            PlayMusic(clip);
            yield return FadeIn();
        }
    }

    private IEnumerator FadeOut()
    {
        if (sourceMusic.volume <= 0.0f)
        {
            FadingOut = null;
            yield break;
        }

        FloatTween tween = TweenFactory.Tween(null, sourceMusic.volume, 0, FadeDelay, TweenScaleFunctions.Linear, null);

        sourceMusic.volume = 1.0f;

        while (!tween.Update(Time.deltaTime))
        {
            sourceMusic.volume = tween.CurrentValue;
            yield return new WaitForEndOfFrame();
        }

        sourceMusic.volume = 0.0f;
        FadingOut = null;
    }

    private IEnumerator FadeIn()
    {
        if (sourceMusic.volume >= 1.0f)
        {
            FadingIn = null;
            yield break;
        }

        FloatTween tween = TweenFactory.Tween(null, sourceMusic.volume, 1, FadeDelay, TweenScaleFunctions.Linear, null);

        sourceMusic.volume = 0.0f;

        while (!tween.Update(Time.deltaTime))
        {
            sourceMusic.volume = tween.CurrentValue;
            yield return new WaitForEndOfFrame();
        }

        sourceMusic.volume = 1.0f;
        FadingIn = null;
    }
}
