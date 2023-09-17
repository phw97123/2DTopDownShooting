using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


/*
 * 발사체 초기화, 업데이트 충돌처리, 파괴를 관리하는 클래스
 */
public class RangedAttackController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private RangedAttackData _attackData;
    private float _currentDuration; //생성 이후로 경과한 시간
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

        //현재 지속시간이 설정한 시간보다 크면 발사체 비활성화
        if (_currentDuration > _attackData.duration)
        {
            DestroyProjectile(transform.position, false);
        }

        //발사체를 주어진 방향과 속도로 움직이도록 설정
        _rigidbody.velocity = _direction * _attackData.speed;
    }

    //현재 오브젝트와 다른 레이어 간의 충돌 검사
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //levelCollisionLayer에 정의된 레이어와 충돌한 객체의 레이어가 동일하다면 
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

    //오브젝트 초기화
    public void InitializeAttack(Vector2 direction, RangedAttackData attackData, ProjectileManager projectileManager)
    {
        //ProjectileManager 및 공격 데이터 초기화
        _projectileManager = projectileManager;
        _attackData = attackData;
        _direction = direction;

        UpdateProjectileSprite();

        //발사체의 궤적을 나타내는 TrailRenderer를 초기화하고 그려진 트레일 제거
        _trailRenderer.Clear();

        //발사체의 현재 지속 시간 초기화 
        _currentDuration = 0;

        //발사체 스프라이트 색상 설정
        _spriteRenderer.color = attackData.projectileColor;

        //발사체의 방향 설정 
        //x축 방향을 (내 오른쪽을) _direction으로 맞춰준다
        transform.right = _direction;

        //발사체가 준비된 상태
        _isReady = true;
    }

    //발사체 스프라이트 업데이트
    private void UpdateProjectileSprite()
    {
        //공격 데이터에서 정의한 크기로 업데이트
        transform.localScale = Vector3.one * _attackData.size;
    }

    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        if (createFx)
        {
            //발사체 파괴 시 효과 생성 
            _projectileManager.CreateImpactParticlesAtPostion(position, _attackData); 
        }
        gameObject.SetActive(false);
    }

}
