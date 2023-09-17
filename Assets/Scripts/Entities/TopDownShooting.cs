
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/*
 * �߻絿�� ���� ( �߻�ü�� ���� �� �л��� �����ϸ�, ���ʹϾ��� ����Ͽ� ���͸� ȸ����Ű�� ����� ����
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

    //�߻�ü ���� ������ �������� �л굵�� ����Ͽ� ���� �߻�ü�� ����
    private void OnShoot(AttackSO attackSO) 
    {
        RangedAttackData rangedAttackData = attackSO as RangedAttackData;
        
        //���� �߻�ü�� ����
        float projectilesAngleSpace = rangedAttackData.multipleProjectilesAngle;

        //�� ���� �߻��� �߻�ü ��
        int numberOfProjectilePerShot = rangedAttackData.numberOfProjectilesPershot;

        //�߻�ü�� �ְ� ����
        //��ü �߻�ü ���� 2�� ������ �߾ӿ� ��ġ�� �߻�ü�� �ε��� 
        //-��ȣ�� �پ������Ƿ� �߾��� �߽����� �߻�ü���� ����
        //�߻�ü���� �� ���� ������ ������ 0.5f�� ���Ѵ� �̷��� �ϸ� �߾ӿ� ��ġ�� �߻�ü�� ��Ȯ�ϰ� �߾����� ������ �����ȴ�
        float minAngle = -(numberOfProjectilePerShot / 2f) * projectilesAngleSpace + 0.5f;
        
        //���� �߻�ü�� �ݺ��Ͽ� �߻��Ѵ�
        for (int i = 0; i < numberOfProjectilePerShot; i++)
        {
            //���� �߻�ü�� ������ ���
            float angle = minAngle + projectilesAngleSpace * i;
            
            //�߻�ü�� ������ �������� �л굵(random spread) �� ���Ѵ� 
            float randomSpread = Random.Range(-rangedAttackData.spread, rangedAttackData.spread);
            angle += randomSpread; 

            //�߻�ü ����
            CreateProjectile(rangedAttackData, angle);
        }
    }

    //�߻�ü�� ������ ���� projectileManager���� �߻�ü ���� ���� 
    private void CreateProjectile(RangedAttackData rangedAttackData,float angle)
    {
        _projectileManager.ShootBullet(
            projectileSpawnPosition.position,     //�߻���ġ
            RotateVector2(_aimDirection,angle),   //ȸ����
            rangedAttackData                      //���� ����
            );

        if (shootingClip)
            SoundManager.PlayClip(shootingClip); 

    }

    //�߻�ü�� ȸ������ ���
    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        //z���� �������� �־��� ����(degree) ��ŭ ȸ���� Quaternion�� v�� ���ؼ� �־��� ����(degree) ��ŭ ȸ���� ���ο� ���Ͱ� �����ȴ�  
        return Quaternion.Euler(0, 0, degree) * v; 
    }

    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection; 
    }
}
