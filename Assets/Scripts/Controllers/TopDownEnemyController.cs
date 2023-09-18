using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터의 기본 제어와 동작을 관리
public class TopDownEnemyController : TopDownCharacterController
{
    GameManager gameManager;

    // 적 캐릭터가 추적하고 있는 가장 가까운 대상
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
        //나와 가까운 오브젝트 탐색
        return Vector3.Distance(transform.position, ClosestTarget.position); 
    }

    protected Vector2 DirectionToTarget()
    {
        //내위치에서 다른 오브젝트를 바라보는 방향 
        return (ClosestTarget.position - transform.position).normalized; 
    }
}
