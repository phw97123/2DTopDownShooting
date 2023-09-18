using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

//효과음과 배경음악 관리
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    //효과음 볼륨 조절을 위한 변수
    [SerializeField][Range(0f, 1f)] private float soundEffectVolume;
    //효과음 피치 변화를 위한 변수 
    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance;
    //배경 음악 볼륨 조절을 위한 변수 
    [SerializeField][Range(0f, 1f)] private float musicVolume;
    private ObjectPool objectPool;

    private AudioSource musicAudioSource;
    public AudioClip musicClip;

    private void Awake()
    {
        instance = this;
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.volume = musicVolume;
        musicAudioSource.loop = true;

        //효과음 재생을 위한 오브젝트 풀을 가져옴
        objectPool = GetComponent<ObjectPool>();
    }

    private void Start()
    {
        ChangeBackGroundMusic(musicClip); 
    }

    private void ChangeBackGroundMusic(AudioClip music)
    {
        instance.musicAudioSource.Stop();
        instance.musicAudioSource.clip = music;
        instance.musicAudioSource.Play(); 
    }

    // 외부에서 호출하여 AudioClip을 재생하는 메서드
    public static void PlayClip(AudioClip clip)
    {
        GameObject obj = instance.objectPool.SpawnFromPool("SoundSource");
        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, instance.soundEffectVolume, instance.soundEffectPitchVariance); 
    }
}
