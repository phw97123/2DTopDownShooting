using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ȿ������ ����ϰ� ����
public class SoundSource : MonoBehaviour
{
    private AudioSource _audioSource;

    // �ܺο��� ȣ���Ͽ� AudioClip�� ����ϰ� ȿ���� �Ӽ�(���� �� ��ġ)�� �����ϴ� �޼���
    public void Play(AudioClip clip, float SoundEffectVolume, float soundEffectPitchVariance)
    {
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        CancelInvoke();
        _audioSource.clip = clip;
        _audioSource.volume = SoundEffectVolume;
        _audioSource.Play();

        // ��ġ ���� �������� ����
        _audioSource.pitch = 1f + Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);

        // ������ �ð� �Ŀ� Disable �޼��带 ȣ���Ͽ� ������Ʈ�� ��Ȱ��ȭ
        Invoke("Disable", clip.length + 2); 
    }

    public void Disable()
    {
        _audioSource.Stop();
        gameObject.SetActive(false); 
    }
}
