
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/*
 * 발사동작 관리 ( 발사체의 방향 및 분산을 설정하며, 쿼터니언을 사용하여 벡터를 회전시키는 기능을 제공
 */
public class TopDownShooting : MonoBehaviour
{
    private ProjectileManager _projectileManager; 
    private TopDownCharacterController _controller;

    [SerializeField] private Transform projectileSpawnPosition;
    private Vector2 _aimDirection = Vector2.right;

    public AudioClip shootingClip;
    private void Awake()
    {
        _controller = GetComponent<TopDownCharacterController>();
    }

    void Start()
    {
        _projectileManager = ProjectileManager.instance; 
        _controller.OnAttackEvent += OnShoot;
        _controller.OnLookEvent += OnAim; 
    }

    //발사체 간의 각도와 무작위한 분산도를 사용하여 여러 발사체를 생성
    private void OnShoot(AttackSO attackSO) 
    {
        RangedAttackData rangedAttackData = attackSO as RangedAttackData;
        
        //여러 발사체의 간격
        float projectilesAngleSpace = rangedAttackData.multipleProjectilesAngle;

        //한 번에 발사할 발사체 수
        int numberOfProjectilePerShot = rangedAttackData.numberOfProjectilesPershot;

        //발사체의 최고 각도
        //전체 발사체 수를 2로 나누면 중앙에 위치한 발사체의 인덱스 
        //-부호가 붙어있으므로 중앙을 중심으로 발사체들을 정렬
        //발사체들의 총 각도 범위의 절반인 0.5f를 더한다 이렇게 하면 중앙에 위치한 발사체가 정확하게 중앙으로 오도록 보정된다
        float minAngle = -(numberOfProjectilePerShot / 2f) * projectilesAngleSpace + 0.5f;
        
        //여러 발사체를 반복하여 발사한다
        for (int i = 0; i < numberOfProjectilePerShot; i++)
        {
            //현재 발사체의 각도를 계산
            float angle = minAngle + projectilesAngleSpace * i;
            
            //발사체의 각도에 무작위한 분산도(random spread) 를 더한다 
            float randomSpread = Random.Range(-rangedAttackData.spread, rangedAttackData.spread);
            angle += randomSpread; 

            //발사체 생성
            CreateProjectile(rangedAttackData, angle);
        }
    }

    //발사체를 생성을 위해 projectileManager에게 발사체 정보 전달 
    private void CreateProjectile(RangedAttackData rangedAttackData,float angle)
    {
        _projectileManager.ShootBullet(
            projectileSpawnPosition.position,     //발사위치
            RotateVector2(_aimDirection,angle),   //회전각
            rangedAttackData                      //공격 정보
            );

        if (shootingClip)
            SoundManager.PlayClip(shootingClip); 

    }

    //발사체의 회전각을 계산
    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        //z축을 기준으로 주어진 각도(degree) 만큼 회전한 Quaternion에 v를 곱해서 주어진 각도(degree) 만큼 회전한 새로운 벡터가 생성된다  
        return Quaternion.Euler(0, 0, degree) * v; 
    }

    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection; 
    }
}
