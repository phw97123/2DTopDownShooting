using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//사거리 공격을 수행하는 몬스터 클래스 
public class TopDownRangeEnemyController : TopDownEnemyController
{
    //추적하는 거리 
    [SerializeField] private float followRange = 15f;

    //공격 가능한 거리
    [SerializeField] private float shootRange = 10f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        IsAttacking = false; 
        if(distance <= followRange) //대상을 추적하는 거리 범위 내에 있는 경우
        {
            if(distance <= shootRange) //공격 가능한 사거리 내에 있는 경우
            {
                int layerMaskTarget = Stats.CurrentStats.attackSO.target;

                //자기 자신과 player 사이에 막혀있는 지형이 있는지 확인
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 11f, (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget); 

                if(hit.collider != null && layerMaskTarget == (layerMaskTarget | 1<< hit.collider.gameObject.layer))
                {
                    CallLookEvent(direction);
                    CallMoveEvent(Vector2.zero); //이동하지 않음
                    IsAttacking = true; 
                }
                else
                {
                    CallMoveEvent(direction); 
                }

            }
            else
            {
                CallMoveEvent(direction);
            }
        }
        else
        {
            CallMoveEvent(direction); 
        }
    }
}
