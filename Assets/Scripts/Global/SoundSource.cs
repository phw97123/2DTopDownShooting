using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//효과음을 재생하고 관리
public class SoundSource : MonoBehaviour
{
    private AudioSource _audioSource;

    // 외부에서 호출하여 AudioClip을 재생하고 효과음 속성(볼륨 및 피치)을 설정하는 메서드
    public void Play(AudioClip clip, float SoundEffectVolume, float soundEffectPitchVariance)
    {
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        CancelInvoke();
        _audioSource.clip = clip;
        _audioSource.volume = SoundEffectVolume;
        _audioSource.Play();

        // 피치 값을 랜덤으로 조절
        _audioSource.pitch = 1f + Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);

        // 지정된 시간 후에 Disable 메서드를 호출하여 오브젝트를 비활성화
        Invoke("Disable", clip.length + 2); 
    }

    public void Disable()
    {
        _audioSource.Stop();
        gameObject.SetActive(false); 
    }
}
