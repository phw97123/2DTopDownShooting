using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//전체적인 발사체를 관리하는 매니저 
public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _impactParticleSystem;
    public static ProjectileManager instance;

    private ObjectPool objectPool; 

    private void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        objectPool = GetComponent<ObjectPool>(); 
    }

    //발사체를 생성하고 초기화
    public void ShootBullet(Vector2 startPostion, Vector2 direction, RangedAttackData attackData)
    {
        //오브젝트 풀을 이용하여 프리팹 생성 
        GameObject obj = objectPool.SpawnFromPool(attackData.bulletNameTag); 

        //생성된 오브젝트의 위치를 시작 위치로 설정 
        obj.transform.position = startPostion;
        RangedAttackController attackController = obj.GetComponent<RangedAttackController>();
        
        //방향, 공격데이터, 현재 스크립트의 참조 전달
        attackController.InitializeAttack(direction, attackData, this);

        //생성된 오브젝트를 활성화
        obj.SetActive(true); 
    }

    //지정된 위치에 공격 이펙트 파티클을 생성하는 함수
    public void CreateImpactParticlesAtPostion(Vector3 position, RangedAttackData attackData)
    {
        //파티클 시스템의 위치를 지정된 위치로 설정
        _impactParticleSystem.transform.position = position;

        //파티클 시스템의 발사 모듈을 가져옴
        ParticleSystem.EmissionModule em = _impactParticleSystem.emission;

        //파티클의 발사 량을 설정 공격 데이터의 크기에 따라 파티클 수를 조정
        em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(attackData.size * 5)));

        //파티클 시스템의 주요 모듈을 가져옴
        ParticleSystem.MainModule mainModule = _impactParticleSystem.main;

        //파티클의 초기 속도를 설정합니다. 공격 데이터의 크기에 따라 속도를 조정
        mainModule.startSpeedMultiplier = attackData.size * 10f;

        _impactParticleSystem.Play(); 
    }

}
