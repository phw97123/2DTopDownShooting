using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Player와 충돌하면 데미지를 주는 몬스터 클래스
public class TopDownContactEnemyController : TopDownEnemyController
{
    //대상을 추적하는 거리 
    [SerializeField][Range(0f, 100f)] private float followRange;

    //대상의 태그
    [SerializeField] private string targetTag = "Player";
    
    //충돌중인지 여부
    private bool _isCollidingWithTarget;

    [SerializeField] private SpriteRenderer characterRenderer;

    //자신의 HealthSystem
    private HealthSystem healthSystem;

    // 대상의 HealthSystem 클래스의 인스턴스를 참조
    private HealthSystem _collidingTargetHealSystem;

    //대상의 이동을 제어하는 TopDownMovement 클래스의 인스턴스
    private TopDownMovement _collidingMovement; 

    protected override void Start()
    {
        base.Start();
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamage += OnDamage; 
    }

    //데미지를 받았을 땐 따라오는 거리를 넓혀서 따라오게 만듬
    private void OnDamage()
    {
        followRange = 100f; 
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //대상과 접촉 중인 경우
        if(_isCollidingWithTarget)
        {
            ApplyHealthChange(); 
        }

        Vector2 direction = Vector2.zero;

        // 대상과의 거리가 추적 범위 내에 있으면 대상을 향해 이동합니다.
        if (DistanceToTarget() <followRange)
        {
            direction = DirectionToTarget(); 
        }

        CallMoveEvent(direction);
        Rotate(direction); 
    }

    //대상 쪽을 바라보도록 캐릭터 회전
    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 대상 쪽이 캐릭터의 왼쪽에 있는지 여부에 따라 스프라이트를 뒤집습니다.
        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f; 
    }

    //대상과 충돌했을 경우
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject receiver = collision.gameObject; 
        if(!receiver.CompareTag(targetTag))
        {
            return; 
        }
        _collidingTargetHealSystem = receiver.GetComponent<HealthSystem>(); 
        if(_collidingTargetHealSystem != null)
        {
            _isCollidingWithTarget = true; 
        }
        _collidingMovement = receiver.GetComponent<TopDownMovement>(); 
    }

    //대상과 접촉이 끝난 경우 
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.CompareTag(targetTag))
        {
            return; 
        }

        _isCollidingWithTarget = false;
    }

    //대상에게 ㄷ미지를 입히고 넉백을 적용하는 메서드
    private void ApplyHealthChange()
    {
        AttackSO attackSO = Stats.CurrentStats.attackSO;
        bool hasBeenChanged = _collidingTargetHealSystem.ChangeHealth(-attackSO.power); 
        if(attackSO.isOnKnockback && _collidingMovement != null)
        {
            _collidingMovement.ApplyKnockback(transform, attackSO.knockbackPower, attackSO.knockbackTime); 
        }
    }
}
