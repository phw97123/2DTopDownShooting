using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//ĳ������ Ư�� ������ �����ϴ� ���� ���� �������� ó���ϴ� Ŭ����
public class PickupStatModifiers : PickupItem
{
    // �������� �ֿ����� �� ������ ���� ���� ���
    [SerializeField] private List<CharacterStats> statsModifier;
    protected override void OnPickedUp(GameObject receiver)
    {
        CharacterStatsHandler statsHandler = receiver.GetComponent<CharacterStatsHandler>();
        foreach (CharacterStats stat in statsModifier)
        {
            if (item == null)
                statsHandler.AddStatModifier(stat);
            else
                Inventory.instance.AddItem(item, stat);

        }

    }
}
