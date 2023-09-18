using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static UnityEngine.GraphicsBuffer;

//ĳ������ ���Ȱ� ���� ������ ���� 
public class CharacterStatsHandler : MonoBehaviour
{ 
    //���Ȱ� ���� �������� �ּ� �� ���
    private const float MinAttackDelay = 0.03f;
    private const float MinAttackPower = 0.5f;
    private const float MinAttackSize = 0.4f;
    private const float MinAttackSpeed = 0.1f;

    private const float MinSpeed = 0.8f;
    private const int MinMaxHealth = 5;

    //�⺻ ���� ������ �����ϴ� ����
    [SerializeField] private CharacterStats baseStats;

    public CharacterStats CurrentStats { get; private set; }

    // ���� ���� ������ �����ϴ� ����Ʈ
    public List<CharacterStats> StatsModifiers = new List<CharacterStats>();

    private void Awake()
    {
        UpdateCharacterStats();
    }

    // ���� ���� ������ �߰��ϴ� �޼���
    public void AddStatModifier(CharacterStats statModifier)
    {
        StatsModifiers.Add(statModifier);
        UpdateCharacterStats();
    }

    // ���� ���� ������ �����ϴ� �޼���
    public void RemoveStatModifier(CharacterStats statModifier)
    {
        StatsModifiers.Remove(statModifier);
        UpdateCharacterStats();
    }

    // ĳ������ ������ ����ϰ� ������Ʈ�ϴ� �޼����, �⺻ ���Ȱ� ���� ���� ������ ����Ͽ� ���� ������ ���
    private void UpdateCharacterStats()
    {
        //�⺻ ���� �����͸� �����Ͽ� ���� ������ ���� �����ͷ� ����
        AttackSO attackSO = null;
        if (baseStats.attackSO != null)
        {
            attackSO = Instantiate(baseStats.attackSO);
        }

        //���� ���� ��ü�� �����ϰ� ���� �����͸� ����
        CurrentStats = new CharacterStats { attackSO = attackSO };

        //�⺻ ������ ����Ͽ� ���� ������ ������Ʈ
        //�̶�, ���� ���� ������ ���� �����(Override), ���ϱ�(Add), Ȥ�� ���ϱ�(Multiple)�� ����
        UpdateStats((a, b) => b, baseStats);

        //���� ���� �������� ���(target)�� �⺻ ������ ���� �����Ϳ� �����ϰ� ����
        if (CurrentStats.attackSO != null)
        {
            CurrentStats.attackSO.target = baseStats.attackSO.target; 
        }

        //���� ���� ������ ��ȸ�ϸ鼭 ���� ���ȿ� �ݿ�
        foreach (CharacterStats modifier in StatsModifiers.OrderBy(o => o.statsChangeType))
        {
            // �����(Override) ���� ������ ���, ������ ���ο� ������ ����
            if (modifier.statsChangeType == StatsChangeType.Override)
            {
                UpdateStats((o, o1) => o1, modifier); 
            }
            // ���ϱ�(Add) ���� ������ ���, ������ ���� ���� ����
            else if (modifier.statsChangeType == StatsChangeType.Add)
            {
                UpdateStats((o, o1) => o + o1, modifier); 
            }
            // ���ϱ�(Multiple) ���� ������ ���, ������ ���� ���� ����
            else if (modifier.statsChangeType == StatsChangeType.Multiple)
            {
                UpdateStats((o, o1) => o * o1, modifier); 
            }
        }

        //��� ������ �ּҰ� �̻����� �����մϴ�.
        LimitAllStats(); 
    }

    //���� ĳ���� ������ ������Ʈ, ������ �������� ����
    private void UpdateStats(Func<float, float, float> operation, CharacterStats newModifier)
    {
        //���� ������ �ִ� ü��(maxHealth)�� �̵� �ӵ�(speed)�� operation ���� �Լ��� ����Ͽ� ������Ʈ
        CurrentStats.maxHealth = (int)operation(CurrentStats.maxHealth, newModifier.maxHealth);
        CurrentStats.speed = operation(CurrentStats.speed, newModifier.speed);

        //���� ������ ���� ������(attackSO)�� ���ο� ���� ���� ������ ���� �����͸� Ȯ��
        //�� �� �ϳ��� null�̸� �޼��带 ����
        if (CurrentStats.attackSO == null || newModifier.attackSO == null)
            return;

        //���� ������ ���� �����͸� ������Ʈ
        UpdateAttackStats(operation, CurrentStats.attackSO, newModifier.attackSO);

        //���� ������ ���� �����Ϳ� ���ο� ���� ���� ������ ���� �������� ������ ��
        //���� ���� ������ �ٸ��� ������Ʈ�� ����
        if (CurrentStats.attackSO.GetType() != newModifier.attackSO.GetType())
        {
            return;
        }

        //���� ���� �������� ������ RangedAttackData Ŭ������ ���, �߰����� ������Ʈ�� �����ϱ� ���� `ApplyRangedStats()` �޼��带 ȣ��
        switch (CurrentStats.attackSO)
        {
            case RangedAttackData _: // ���ϸ�Ī?
                ApplyRangedStats(operation, newModifier);
                break;
        }
    }

    // ���� �������� ������ �����ϴ� �޼���
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

    // ���Ÿ� ���� �������� ������ �����ϴ� �޼���
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

    // ���� ���� �����ϴ� �޼���
    private Color UpdateColor(Func<float, float, float> operation, Color currentColor, Color newColor)
    {
        return new Color(
            operation(currentColor.r, newColor.r),
             operation(currentColor.g, newColor.g),
              operation(currentColor.b, newColor.b),
               operation(currentColor.a, newColor.a)
              );
    }

    // ������ �ּҰ����� �����ϴ� �޼���
    private void LimitStats(ref float stat, float minVal)
    {
        stat = Mathf.Max(stat, minVal); 
    }

    // ��� ������ �ּҰ����� �����ϴ� �޼���
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
