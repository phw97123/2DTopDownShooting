using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//게임 캐릭터의 제어와 이벤트 처리
public class TopDownCharacterController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action<AttackSO> OnAttackEvent;

    // 마지막 공격 이후로 경과한 시간
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

        //IsAttacking이 true이고, 지정된 딜레이 이후인 경우, 공격 이벤트를 호출하고 타이머를 리셋
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


