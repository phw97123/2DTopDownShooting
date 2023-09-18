using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//걸음 효과(걸을 때 먼지나는 이펙트)를 제어하는 클래스
public class DustParticleControl : MonoBehaviour 
{
    //걷기 동작시 파티클을 생성할지 여부를 설정하는 변수
    [SerializeField] private bool createDustOnWalk = true;
    [SerializeField] private ParticleSystem dustParticleSystem; 

    //파티클 생성
    public void CreteDustParticles()
    {
        if(createDustOnWalk)
        {
            //정지하고 다시 재생함으로써 효과를 초기화 하고 새로운 효과를 나타낸다
            dustParticleSystem.Stop();
            dustParticleSystem.Play(); 
        }
    }
}
