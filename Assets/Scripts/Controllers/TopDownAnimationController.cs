using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


//ĳ������ �ִϸ��̼��� ��ü������ ���� 
public class TopDownAnimationController : TopDownAnimations
{
    //StringToHash : Ư�� ���ڿ��� ���ڰ�(�ؽð�)���� ��ȯ 
    //���ڿ� ������� ����� ���Ƽ� �ؽð����� ���ϰ� �ϴ� �� 
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int IsHit = Animator.StringToHash("IsHit");

    private HealthSystem _healthSystem; 

    protected override void Awake()
    {
        base.Awake();
        _healthSystem = GetComponent<HealthSystem>(); 
    }

    void Start()
    {
        controller.OnAttackEvent += Attacking;
        controller.OnMoveEvent += Move; 

        if(_healthSystem != null)
        {
            _healthSystem.OnDamage += Hit;
            _healthSystem.OnInvincibilityEnd += InvincibilityEnd; 
        }
    }

    private void Move(Vector2 obj)
    {
        //ũ�Ⱑ 0.5���� ũ�� true; 
        animator.SetBool(IsWalking, obj.magnitude >.5f); 
    }

    private void Attacking(AttackSO obj)
    {   
        //Ȯ�θ� ���ش� �״ٰ� ����
        animator.SetTrigger(Attack); 
    }

    private void Hit()
    {
        animator.SetBool(IsHit, true); 
    }

    private void InvincibilityEnd()
    {
        animator.SetBool(IsHit, false);
    }
}
