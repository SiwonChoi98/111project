using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BGMStruct
{
    public string name;
    public AudioClip clip;
}
[Serializable]
public class SFXStruct
{
    public string name;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;
    public List<BGMStruct> bgmSoundList;
    public List<SFXStruct> sfxSoundList;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    //배경음악
    public void BgmPlaySound(int index, float volume = 1f)
    {
        bgmAudioSource.clip = bgmSoundList[index].clip;
        bgmAudioSource.volume = volume;
        bgmAudioSource.Play();
        
    }
    //효과음
    public void SfxPlaySound(int index, float volume = 1f)
    {
        sfxAudioSource.clip = sfxSoundList[index].clip;
        sfxAudioSource.PlayOneShot(sfxAudioSource.clip, volume);
    }

}
