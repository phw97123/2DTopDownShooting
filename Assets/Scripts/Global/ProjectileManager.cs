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

}
