using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//체력을 회복하는 힐 아이템을 처리하는 클래스
public class PickupHeal : PickupItem
{
    [SerializeField] int healValue = 10;

    //회복 대상의 healthSystem
    private HealthSystem _healthSystem; 

    protected override void OnPickedUp(GameObject receiver)
    {
        _healthSystem = receiver.GetComponent<HealthSystem>();
        // 회복 대상의 체력을 지정된 양만큼 회복시킴
        _healthSystem.ChangeHealth(healValue); 
    }
}
