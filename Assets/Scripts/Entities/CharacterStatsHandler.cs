using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static UnityEngine.GraphicsBuffer;

//캐릭터의 스탯과 공격 데이터 관리 
public class CharacterStatsHandler : MonoBehaviour
{ 
    //스탯과 공격 데이터의 최소 값 상수
    private const float MinAttackDelay = 0.03f;
    private const float MinAttackPower = 0.5f;
    private const float MinAttackSize = 0.4f;
    private const float MinAttackSpeed = 0.1f;

    private const float MinSpeed = 0.8f;
    private const int MinMaxHealth = 5;

    //기본 스탯 정보를 저장하는 변수
    [SerializeField] private CharacterStats baseStats;

    public CharacterStats CurrentStats { get; private set; }

    // 스탯 수정 정보를 저장하는 리스트
    public List<CharacterStats> StatsModifiers = new List<CharacterStats>();

    private void Awake()
    {
        UpdateCharacterStats();
    }

    // 스탯 수정 정보를 추가하는 메서드
    public void AddStatModifier(CharacterStats statModifier)
    {
        StatsModifiers.Add(statModifier);
        UpdateCharacterStats();
    }

    // 스탯 수정 정보를 제거하는 메서드
    public void RemoveStatModifier(CharacterStats statModifier)
    {
        StatsModifiers.Remove(statModifier);
        UpdateCharacterStats();
    }

    // 캐릭터의 스탯을 계산하고 업데이트하는 메서드로, 기본 스탯과 스탯 수정 정보를 고려하여 최종 스탯을 계산
    private void UpdateCharacterStats()
    {
        //기본 공격 데이터를 복제하여 현재 스탯의 공격 데이터로 설정
        AttackSO attackSO = null;
        if (baseStats.attackSO != null)
        {
            attackSO = Instantiate(baseStats.attackSO);
        }

        //현재 스탯 객체를 생성하고 공격 데이터를 설정
        CurrentStats = new CharacterStats { attackSO = attackSO };

        //기본 스탯을 사용하여 현재 스탯을 업데이트
        //이때, 스탯 변경 유형에 따라 덮어쓰기(Override), 더하기(Add), 혹은 곱하기(Multiple)를 수행
        UpdateStats((a, b) => b, baseStats);

        //현재 공격 데이터의 대상(target)을 기본 스탯의 공격 데이터와 동일하게 설정
        if (CurrentStats.attackSO != null)
        {
            CurrentStats.attackSO.target = baseStats.attackSO.target; 
        }

        //스탯 수정 정보를 순회하면서 현재 스탯에 반영
        foreach (CharacterStats modifier in StatsModifiers.OrderBy(o => o.statsChangeType))
        {
            // 덮어쓰기(Override) 변경 유형인 경우, 스탯을 새로운 값으로 설정
            if (modifier.statsChangeType == StatsChangeType.Override)
            {
                UpdateStats((o, o1) => o1, modifier); 
            }
            // 더하기(Add) 변경 유형인 경우, 스탯을 현재 값에 더함
            else if (modifier.statsChangeType == StatsChangeType.Add)
            {
                UpdateStats((o, o1) => o + o1, modifier); 
            }
            // 곱하기(Multiple) 변경 유형인 경우, 스탯을 현재 값에 곱함
            else if (modifier.statsChangeType == StatsChangeType.Multiple)
            {
                UpdateStats((o, o1) => o * o1, modifier); 
            }
        }

        //모든 스탯을 최소값 이상으로 제한합니다.
        LimitAllStats(); 
    }

    //현재 캐릭터 스탯을 업데이트, 스탯을 동적으로 조절
    private void UpdateStats(Func<float, float, float> operation, CharacterStats newModifier)
    {
        //현재 스탯의 최대 체력(maxHealth)과 이동 속도(speed)를 operation 연산 함수를 사용하여 업데이트
        CurrentStats.maxHealth = (int)operation(CurrentStats.maxHealth, newModifier.maxHealth);
        CurrentStats.speed = operation(CurrentStats.speed, newModifier.speed);

        //현재 스탯의 공격 데이터(attackSO)와 새로운 스탯 수정 정보의 공격 데이터를 확인
        //둘 중 하나라도 null이면 메서드를 종료
        if (CurrentStats.attackSO == null || newModifier.attackSO == null)
            return;

        //현재 스탯의 공격 데이터를 업데이트
        UpdateAttackStats(operation, CurrentStats.attackSO, newModifier.attackSO);

        //현재 스탯의 공격 데이터와 새로운 스탯 수정 정보의 공격 데이터의 유형을 비교
        //만약 둘의 유형이 다르면 업데이트를 종료
        if (CurrentStats.attackSO.GetType() != newModifier.attackSO.GetType())
        {
            return;
        }

        //현재 공격 데이터의 유형이 RangedAttackData 클래스인 경우, 추가적인 업데이트를 수행하기 위해 `ApplyRangedStats()` 메서드를 호출
        switch (CurrentStats.attackSO)
        {
            case RangedAttackData _: // 패턴매칭?
                ApplyRangedStats(operation, newModifier);
                break;
        }
    }

    // 공격 데이터의 스탯을 수정하는 메서드
    private void UpdateAttackStats(Func<float, float, float> operation, AttackSO currentAttack, AttackSO newAttack)
    {
        if (currentAttack == null || newAttack == null)
        {
            return;
        }

        currentAttack.delay = operation(currentAttack.delay, newAttack.delay);
        currentAttack.power = operation(currentAttack.power, newAttack.power);
        currentAttack.size = operation(currentAttack.size, newAttack.size);
        currentAttack.speed = operation(currentAttack.speed, newAttack.speed);
    }

    // 원거리 공격 데이터의 스탯을 수정하는 메서드
    private void ApplyRangedStats(Func<float, float, float> operation, CharacterStats newModifier)
    {
        RangedAttackData currentRangedAttacks = (RangedAttackData)CurrentStats.attackSO;

        if (!(newModifier.attackSO is RangedAttackData))
        {
            return;
        }

        RangedAttackData rangedAttacksModifier = (RangedAttackData)newModifier.attackSO;
        currentRangedAttacks.multipleProjectilesAngle = operation(currentRangedAttacks.multipleProjectilesAngle, rangedAttacksModifier.multipleProjectilesAngle);
        currentRangedAttacks.spread = operation(currentRangedAttacks.spread, rangedAttacksModifier.spread);
        currentRangedAttacks.duration = operation(currentRangedAttacks.duration, rangedAttacksModifier.duration);
        currentRangedAttacks.numberOfProjectilesPershot = Mathf.CeilToInt(operation(currentRangedAttacks.numberOfProjectilesPershot, rangedAttacksModifier.numberOfProjectilesPershot));
        currentRangedAttacks.projectileColor = UpdateColor(operation, currentRangedAttacks.projectileColor, rangedAttacksModifier.projectileColor);
    }

    // 색상 값을 수정하는 메서드
    private Color UpdateColor(Func<float, float, float> operation, Color currentColor, Color newColor)
    {
        return new Color(
            operation(currentColor.r, newColor.r),
             operation(currentColor.g, newColor.g),
              operation(currentColor.b, newColor.b),
               operation(currentColor.a, newColor.a)
              );
    }

    // 스탯을 최소값으로 제한하는 메서드
    private void LimitStats(ref float stat, float minVal)
    {
        stat = Mathf.Max(stat, minVal); 
    }

    // 모든 스탯을 최소값으로 제한하는 메서드
    private void LimitAllStats()
    {
        if(CurrentStats == null || CurrentStats.attackSO == null)
        {
            return; 
        }

        LimitStats(ref CurrentStats.attackSO.delay, MinAttackDelay);
        LimitStats(ref CurrentStats.attackSO.power, MinAttackPower);
        LimitStats(ref CurrentStats.attackSO.size, MinAttackSize);
        LimitStats(ref CurrentStats.attackSO.speed, MinAttackSpeed);
        LimitStats(ref CurrentStats.speed, MinSpeed);
        CurrentStats.maxHealth = Mathf.Max(CurrentStats.maxHealth, MinMaxHealth); 
    }
}
