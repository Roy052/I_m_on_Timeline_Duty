using System.Collections;
using UnityEngine;

public enum BGM
{
    Menu,
    Duel,
    Round,
    Upgrade,
}

public enum SFX
{
    None = -1,
    TextTyping,
    Click,
    KaelaScreen,
    Danger,
    Send,
}

public class SoundManager : Singleton
{
    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;

    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    float currentBgmVolume = 0.3f;
    float currentSfxVolume = 0.5f;

    public void Awake()
    {
        if (soundManager == null)
            soundManager = this;
    }

    public void OnDestroy()
    {
        if(soundManager == this)
            soundManager = null;
    }

    public void ChangeBgmVolume(float volume)
    {
        currentBgmVolume = volume;
        bgmAudioSource.volume = volume;
    }

    public void ChangeSfxVolume(float volume)
    {
        currentSfxVolume = volume;
        sfxAudioSource.volume = volume;
    }

    public void PlayBGM(BGM bgm)
    {
        if (bgmClips.Length <= (int)bgm || bgmClips[(int)bgm] == null)
        {
            Debug.LogError($"BGM {bgm} Not Exist");
            return;
        }
        bgmAudioSource.clip = bgmClips[(int)bgm];
        bgmAudioSource.Play();
    }

    Coroutine co_Fade_Bgm = null;

    public void StopBGM()
    {
        if (co_Fade_Bgm != null)
        {
            StopCoroutine(co_Fade_Bgm);
            co_Fade_Bgm = null;
            sfxAudioSource.volume = currentSfxVolume;
        }

        co_Fade_Bgm = StartCoroutine(FadeBGM());
    }

    public void PlaySFX(SFX sfx)
    {
        if (sfx == SFX.None)
            return;

        if (co_Fade_Sfx != null)
        {
            StopCoroutine(co_Fade_Sfx);
            co_Fade_Sfx = null;
            sfxAudioSource.volume = currentSfxVolume;
        }

        if (sfxClips.Length <= (int)sfx || sfxClips[(int)sfx] == null)
        {
            Debug.LogError($"SFX {sfx} Not Exist");
            return;
        }
        sfxAudioSource.clip = sfxClips[(int)sfx];
        sfxAudioSource.Play();
    }

    Coroutine co_Fade_Sfx = null;

    public void PlaySFXLoop(SFX sfx)
    {
        if(co_Fade_Sfx != null)
        {
            StopCoroutine(co_Fade_Sfx);
            co_Fade_Sfx = null;
            sfxAudioSource.volume = currentSfxVolume;
        }

        sfxAudioSource.loop = true;
        PlaySFX(sfx);
    }

    public void StopSFX()
    {
        if (co_Fade_Sfx != null)
        {
            StopCoroutine(co_Fade_Sfx);
            co_Fade_Sfx = null;
            sfxAudioSource.volume = currentSfxVolume;
        }

        sfxAudioSource.loop = false;
        co_Fade_Sfx = StartCoroutine(FadeSFX());
    }

    IEnumerator FadeSFX()
    {
        float time = 0f;
        while(time <= 0.3f)
        {
            sfxAudioSource.volume = Mathf.Lerp(currentSfxVolume, 0, time / 0.3f);
            time += Time.deltaTime;
            yield return null;
        }
        sfxAudioSource.Stop();
        sfxAudioSource.volume = currentSfxVolume;
    }

    IEnumerator FadeBGM()
    {
        float time = 0f;
        while (time <= 0.3f)
        {
            bgmAudioSource.volume = Mathf.Lerp(currentBgmVolume, 0, time / 0.3f);
            time += Time.deltaTime;
            yield return null;
        }
        bgmAudioSource.Stop();
        bgmAudioSource.volume = currentBgmVolume;
    }
}
