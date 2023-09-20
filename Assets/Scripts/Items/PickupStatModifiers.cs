using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//캐릭터의 특정 스탯을 수정하는 스탯 수정 아이템을 처리하는 클래스
public class PickupStatModifiers : PickupItem
{
    // 아이템이 주워졌을 때 적용할 스탯 수정 목록
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
