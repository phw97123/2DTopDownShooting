using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TopDownContactEnemyController : TopDownEnemyController
{
    [SerializeField][Range(0f, 100f)] private float followRange;
    [SerializeField] private string targetTag = "Player";
    private bool _isCollidingWithTarget;

    [SerializeField] private SpriteRenderer characterRenderer;

    private HealthSystem healthSystem;
    private HealthSystem _collidingTargetHealSystem;
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

        if(_isCollidingWithTarget)
        {
            ApplyHealthChange(); 
        }

        Vector2 direction = Vector2.zero; 
        if(DistanceToTarget() <followRange)
        {
            direction = DirectionToTarget(); 
        }

        CallMoveEvent(direction);
        Rotate(direction); 
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f; 
    }

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.CompareTag(targetTag))
        {
            return; 
        }

        _isCollidingWithTarget = false;
    }

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
