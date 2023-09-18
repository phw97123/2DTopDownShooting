using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

//ȿ������ ������� ����
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    //ȿ���� ���� ������ ���� ����
    [SerializeField][Range(0f, 1f)] private float soundEffectVolume;
    //ȿ���� ��ġ ��ȭ�� ���� ���� 
    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance;
    //��� ���� ���� ������ ���� ���� 
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

        //ȿ���� ����� ���� ������Ʈ Ǯ�� ������
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

    // �ܺο��� ȣ���Ͽ� AudioClip�� ����ϴ� �޼���
    public static void PlayClip(AudioClip clip)
    {
        GameObject obj = instance.objectPool.SpawnFromPool("SoundSource");
        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, instance.soundEffectVolume, instance.soundEffectPitchVariance); 
    }
}
