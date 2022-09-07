using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundEffectNames
{
    Click,
    TotalCount
};


public class AudioManager : CustomBehaviour
{
    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;

    public AudioClip[] SoundClips;

    private bool _isSfxEnabled;
    private bool _isMusicEnabled;

    public bool IsSfxEnabled
    {
        get
        {
            if (PlayerPrefs.GetInt("SfxEnabled", 0) == 0)
                return false;
            else
                return true;
        }
        set
        {
            if (value)
                PlayerPrefs.SetInt("SfxEnabled", 1);
            else
                PlayerPrefs.SetInt("SfxEnabled", 0);

            _isSfxEnabled = value;
            sfxAudioSource.mute = !value;
        }
    }

    public bool IsMusicEnabled
    {
        get
        {
            if (PlayerPrefs.GetInt("MusicEnabled", 0) == 0)
                return false;
            else
                return true;
        }
        set
        {
            if (value)
                PlayerPrefs.SetInt("MusicEnabled", 1);
            else
                PlayerPrefs.SetInt("MusicEnabled", 0);

            _isMusicEnabled = value;
            musicAudioSource.mute = !value;
        }
    }

    public bool IsAudioEnabled
    {
        get
        {
            if (PlayerPrefs.GetInt("SfxEnabled", 0) == 1 && PlayerPrefs.GetInt("MusicEnabled", 0) == 1)
                return true;
            else
                return false;
        }
        set
        {
            IsMusicEnabled = value;
            IsSfxEnabled = value;
        }
    }


    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        _isMusicEnabled = IsMusicEnabled;
        _isSfxEnabled = IsSfxEnabled;

        if (_isMusicEnabled)
            musicAudioSource.Play();
    }

    public void PlaySound(SoundEffectNames sfxId)
    {
        if (!_isSfxEnabled)
            return;

        if ((int)sfxId >= SoundClips.Length)
            return;

        if (SoundClips[(int)sfxId] == null)
            return;

        sfxAudioSource.PlayOneShot(SoundClips[(int)sfxId]);
    }

    public void PlayMusic()
    {
        if (!_isSfxEnabled)
            return;

        musicAudioSource.Play();

    }

}