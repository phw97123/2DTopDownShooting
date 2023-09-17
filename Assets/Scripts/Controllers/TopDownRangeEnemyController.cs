using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TopDownRangeEnemyController : TopDownEnemyController
{
    [SerializeField] private float followRange = 15f;
    [SerializeField] private float shootRange = 10f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        IsAttacking = false; 
        if(distance <= followRange) //따라가는 거리보다 작다면
        {
            if(distance <= shootRange) //공격 거리보다 작으면 
            {
                int layerMaskTarget = Stats.CurrentStates.attackSO.target;

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
