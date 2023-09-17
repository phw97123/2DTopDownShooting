using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//��ü���� �߻�ü�� �����ϴ� �Ŵ��� 
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

    //�߻�ü�� �����ϰ� �ʱ�ȭ
    public void ShootBullet(Vector2 startPostion, Vector2 direction, RangedAttackData attackData)
    {
        //������Ʈ Ǯ�� �̿��Ͽ� ������ ���� 
        GameObject obj = objectPool.SpawnFromPool(attackData.bulletNameTag); 

        //������ ������Ʈ�� ��ġ�� ���� ��ġ�� ���� 
        obj.transform.position = startPostion;
        RangedAttackController attackController = obj.GetComponent<RangedAttackController>();
        
        //����, ���ݵ�����, ���� ��ũ��Ʈ�� ���� ����
        attackController.InitializeAttack(direction, attackData, this);

        //������ ������Ʈ�� Ȱ��ȭ
        obj.SetActive(true); 
    }

    public void CreateImpactParticlesAtPostion(Vector3 position, RangedAttackData attackData)
    {
        _impactParticleSystem.transform.position = position;
        ParticleSystem.EmissionModule em = _impactParticleSystem.emission;
        em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(attackData.size * 5)));
        ParticleSystem.MainModule mainModule = _impactParticleSystem.main;
        mainModule.startSpeedMultiplier = attackData.size * 10f;
        _impactParticleSystem.Play(); 
    }

}
