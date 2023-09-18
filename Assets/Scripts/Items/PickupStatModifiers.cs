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

        // 스탯 수정 목록에 있는 각각의 스탯 수정을 적용
        foreach (CharacterStats stat in statsModifier)
        {
            statsHandler.AddStatModifier(stat); 
        }
    }
}
