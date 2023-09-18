using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//��Ÿ� ������ �����ϴ� ���� Ŭ���� 
public class TopDownRangeEnemyController : TopDownEnemyController
{
    //�����ϴ� �Ÿ� 
    [SerializeField] private float followRange = 15f;

    //���� ������ �Ÿ�
    [SerializeField] private float shootRange = 10f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        IsAttacking = false; 
        if(distance <= followRange) //����� �����ϴ� �Ÿ� ���� ���� �ִ� ���
        {
            if(distance <= shootRange) //���� ������ ��Ÿ� ���� �ִ� ���
            {
                int layerMaskTarget = Stats.CurrentStats.attackSO.target;

                //�ڱ� �ڽŰ� player ���̿� �����ִ� ������ �ִ��� Ȯ��
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 11f, (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget); 

                if(hit.collider != null && layerMaskTarget == (layerMaskTarget | 1<< hit.collider.gameObject.layer))
                {
                    CallLookEvent(direction);
                    CallMoveEvent(Vector2.zero); //�̵����� ����
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
