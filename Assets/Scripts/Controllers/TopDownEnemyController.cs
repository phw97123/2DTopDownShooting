using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownEnemyController : TopDownCharacterController
{
    GameManager gameManager; 
    
    //����� ������Ʈ
    protected Transform ClosestTarget { get; private set;  }

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        gameManager = GameManager.instance;
        ClosestTarget = gameManager.Player; 
    }

    protected virtual void FixedUpdate()
    {

    }

    protected float DistanceToTarget()
    {
        //���� ����� ������Ʈ Ž��
        return Vector3.Distance(transform.position, ClosestTarget.position); 
    }

    protected Vector2 DirectionToTarget()
    {
        //����ġ���� �ٸ� ������Ʈ�� �ٶ󺸴� ���� 
        return (ClosestTarget.position - transform.position).normalized; 
    }
}