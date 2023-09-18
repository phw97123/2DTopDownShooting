using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Player�� �浹�ϸ� �������� �ִ� ���� Ŭ����
public class TopDownContactEnemyController : TopDownEnemyController
{
    //����� �����ϴ� �Ÿ� 
    [SerializeField][Range(0f, 100f)] private float followRange;

    //����� �±�
    [SerializeField] private string targetTag = "Player";
    
    //�浹������ ����
    private bool _isCollidingWithTarget;

    [SerializeField] private SpriteRenderer characterRenderer;

    //�ڽ��� HealthSystem
    private HealthSystem healthSystem;

    // ����� HealthSystem Ŭ������ �ν��Ͻ��� ����
    private HealthSystem _collidingTargetHealSystem;

    //����� �̵��� �����ϴ� TopDownMovement Ŭ������ �ν��Ͻ�
    private TopDownMovement _collidingMovement; 

    protected override void Start()
    {
        base.Start();
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamage += OnDamage; 
    }

    //�������� �޾��� �� ������� �Ÿ��� ������ ������� ����
    private void OnDamage()
    {
        followRange = 100f; 
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //���� ���� ���� ���
        if(_isCollidingWithTarget)
        {
            ApplyHealthChange(); 
        }

        Vector2 direction = Vector2.zero;

        // ������ �Ÿ��� ���� ���� ���� ������ ����� ���� �̵��մϴ�.
        if (DistanceToTarget() <followRange)
        {
            direction = DirectionToTarget(); 
        }

        CallMoveEvent(direction);
        Rotate(direction); 
    }

    //��� ���� �ٶ󺸵��� ĳ���� ȸ��
    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ��� ���� ĳ������ ���ʿ� �ִ��� ���ο� ���� ��������Ʈ�� �������ϴ�.
        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f; 
    }

    //���� �浹���� ���
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

    //���� ������ ���� ��� 
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.CompareTag(targetTag))
        {
            return; 
        }

        _isCollidingWithTarget = false;
    }

    //��󿡰� �������� ������ �˹��� �����ϴ� �޼���
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
