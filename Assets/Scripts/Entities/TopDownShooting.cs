
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TopDownShooting : MonoBehaviour
{
    private ProjectileManager _projectileManager; 
    private TopDownCharacterController _controller;

    [SerializeField] private Transform projectileSpawnPosition;
    private Vector2 _aimDirection = Vector2.right;    

    private void Awake()
    {
        _controller = GetComponent<TopDownCharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _projectileManager = ProjectileManager.instance; 
        _controller.OnAttackEvent += OnShoot;
        _controller.OnLookEvent += OnAim; 
    }

    private void OnShoot(AttackSO attackSO)
    {
        RangedAttackData rangedAttackData = attackSO as RangedAttackData;
        float projectilesAngleSpace = rangedAttackData.multipleProjectilesAngel;
        int numberOfProjectilePerShot = rangedAttackData.numverofProjectilesPershot;

        float minAngle = -(numberOfProjectilePerShot / 2f) * projectilesAngleSpace + 0.5f * rangedAttackData.multipleProjectilesAngel;
        
        for (int i = 0; i < numberOfProjectilePerShot; i++)
        {
            float angle = minAngle + projectilesAngleSpace * i;
            float randomSpread = Random.Range(-rangedAttackData.spread, rangedAttackData.spread);
            angle += randomSpread; 
            CreateProjectile(rangedAttackData, angle);

        }

    }

    private void CreateProjectile(RangedAttackData rangedAttackData,float angle)
    {
        _projectileManager.ShootBullet(
            projectileSpawnPosition.position,     //발사위치
            RotateVector2(_aimDirection,angle),   //회전각
            rangedAttackData                      //공격 정보
            );
    }

    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v; 
    }
    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection; 
    }

}
