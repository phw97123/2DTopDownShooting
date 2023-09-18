using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ü���� ȸ���ϴ� �� �������� ó���ϴ� Ŭ����
public class PickupHeal : PickupItem
{
    [SerializeField] int healValue = 10;

    //ȸ�� ����� healthSystem
    private HealthSystem _healthSystem; 

    protected override void OnPickedUp(GameObject receiver)
    {
        _healthSystem = receiver.GetComponent<HealthSystem>();
        // ȸ�� ����� ü���� ������ �縸ŭ ȸ����Ŵ
        _healthSystem.ChangeHealth(healValue); 
    }
}
