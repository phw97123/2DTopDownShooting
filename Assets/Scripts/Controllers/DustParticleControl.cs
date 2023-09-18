using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ȿ��(���� �� �������� ����Ʈ)�� �����ϴ� Ŭ����
public class DustParticleControl : MonoBehaviour 
{
    //�ȱ� ���۽� ��ƼŬ�� �������� ���θ� �����ϴ� ����
    [SerializeField] private bool createDustOnWalk = true;
    [SerializeField] private ParticleSystem dustParticleSystem; 

    //��ƼŬ ����
    public void CreteDustParticles()
    {
        if(createDustOnWalk)
        {
            //�����ϰ� �ٽ� ��������ν� ȿ���� �ʱ�ȭ �ϰ� ���ο� ȿ���� ��Ÿ����
            dustParticleSystem.Stop();
            dustParticleSystem.Play(); 
        }
    }
}
