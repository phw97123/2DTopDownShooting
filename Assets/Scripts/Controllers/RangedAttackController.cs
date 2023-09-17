using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


/*
 * �߻�ü �ʱ�ȭ, ������Ʈ �浹ó��, �ı��� �����ϴ� Ŭ����
 */
public class RangedAttackController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private RangedAttackData _attackData;
    private float _currentDuration; //���� ���ķ� ����� �ð�
    private Vector2 _direction;
    private bool _isReady;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private TrailRenderer _trailRenderer;
    private ProjectileManager _projectileManager;

    public bool fxOnDestroy = true;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (!_isReady)
        {
            return;
        }

        _currentDuration += Time.deltaTime;

        //���� ���ӽð��� ������ �ð����� ũ�� �߻�ü ��Ȱ��ȭ
        if (_currentDuration > _attackData.duration)
        {
            DestroyProjectile(transform.position, false);
        }

        //�߻�ü�� �־��� ����� �ӵ��� �����̵��� ����
        _rigidbody.velocity = _direction * _attackData.speed;
    }

    //���� ������Ʈ�� �ٸ� ���̾� ���� �浹 �˻�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //levelCollisionLayer�� ���ǵ� ���̾�� �浹�� ��ü�� ���̾ �����ϴٸ� 
        if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer)))
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - _direction * .2f, fxOnDestroy);
        }
        else if (_attackData.target.value == (_attackData.target.value | (1 << collision.gameObject.layer)))
        {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>(); 
            if(healthSystem != null)
            {
                healthSystem.ChangeHealth(-_attackData.power); 
                if(_attackData.isOnKnockback)
                {
                    TopDownMovement movement = collision.GetComponent<TopDownMovement>(); 
                    if(movement != null)
                    {
                        movement.ApplyKnockback(transform, _attackData.knockbackPower, _attackData.knockbackTime); 
                    }
                }
            }
            DestroyProjectile(collision.ClosestPoint(transform.position) - _direction * .2f, fxOnDestroy); 
        }

    }

    //������Ʈ �ʱ�ȭ
    public void InitializeAttack(Vector2 direction, RangedAttackData attackData, ProjectileManager projectileManager)
    {
        //ProjectileManager �� ���� ������ �ʱ�ȭ
        _projectileManager = projectileManager;
        _attackData = attackData;
        _direction = direction;

        UpdateProjectileSprite();

        //�߻�ü�� ������ ��Ÿ���� TrailRenderer�� �ʱ�ȭ�ϰ� �׷��� Ʈ���� ����
        _trailRenderer.Clear();

        //�߻�ü�� ���� ���� �ð� �ʱ�ȭ 
        _currentDuration = 0;

        //�߻�ü ��������Ʈ ���� ����
        _spriteRenderer.color = attackData.projectileColor;

        //�߻�ü�� ���� ���� 
        //x�� ������ (�� ��������) _direction���� �����ش�
        transform.right = _direction;

        //�߻�ü�� �غ�� ����
        _isReady = true;
    }

    //�߻�ü ��������Ʈ ������Ʈ
    private void UpdateProjectileSprite()
    {
        //���� �����Ϳ��� ������ ũ��� ������Ʈ
        transform.localScale = Vector3.one * _attackData.size;
    }

    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        if (createFx)
        {
            //�߻�ü �ı� �� ȿ�� ���� 
            _projectileManager.CreateImpactParticlesAtPostion(position, _attackData); 
        }
        gameObject.SetActive(false);
    }

}
