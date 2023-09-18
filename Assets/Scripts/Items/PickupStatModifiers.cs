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

        // ���� ���� ��Ͽ� �ִ� ������ ���� ������ ����
        foreach (CharacterStats stat in statsModifier)
        {
            statsHandler.AddStatModifier(stat); 
        }
    }
}
