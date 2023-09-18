using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���� ĳ������ ����� �̺�Ʈ ó��
public class TopDownCharacterController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action<AttackSO> OnAttackEvent;

    // ������ ���� ���ķ� ����� �ð�
    private float _timeSinceLastAttack = float.MaxValue; 
    protected bool IsAttacking { get; set; }

    protected CharacterStatsHandler Stats { get; private set;  }

    protected virtual void Awake()
    {
        Stats = GetComponent<CharacterStatsHandler>(); 
    }

    protected virtual void Update()
    {
        HandleAttackDelay(); 
    }

    private void HandleAttackDelay()
    {
        if (Stats.CurrentStats.attackSO == null)
            return;

        if(_timeSinceLastAttack <= Stats.CurrentStats.attackSO.delay)
        {
            _timeSinceLastAttack += Time.deltaTime; 
        }

        //IsAttacking�� true�̰�, ������ ������ ������ ���, ���� �̺�Ʈ�� ȣ���ϰ� Ÿ�̸Ӹ� ����
        if (IsAttacking && _timeSinceLastAttack > Stats.CurrentStats.attackSO.delay) 
        {
            _timeSinceLastAttack = 0;
            CallAttackEvnet(Stats.CurrentStats.attackSO); 
        }
    }

    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction); 
    }
    public void CallLookEvent(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }
    public void CallAttackEvnet(AttackSO attackSO)
    {
        OnAttackEvent?.Invoke(attackSO);
    }
}


