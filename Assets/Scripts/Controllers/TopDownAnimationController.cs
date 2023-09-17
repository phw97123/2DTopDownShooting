using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class TopDownAnimationController : TopDownAnimations
{
    //StringToHash : 특정 문자열을 숫자값(해시값)으로 변환 
    //문자열 연산들은 비용이 높아서 해시값으로 비교하게 하는 것 
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int IsHit = Animator.StringToHash("IsHit");

    protected override void Awake()
    {
        base.Awake(); 
    }

    void Start()
    {
        controller.OnAttackEvent += Attacking;
        controller.OnMoveEvent += Move; 
    }

    private void Move(Vector2 obj)
    {
        //크기가 0.5보다 크면 true; 
        animator.SetBool(IsWalking, obj.magnitude >.5f); 
    }

    private void Attacking(AttackSO obj)
    {   
        //확인만 해준다 켰다가 꺼짐
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
