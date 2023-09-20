using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임에서 주울 수 있는 아이템을 처리하기 위한 기본 틀
public abstract class PickupItem : MonoBehaviour
{
    public ItemData item;
    //아이템을 주웠을 때 파괴할지 여부를 결정하는 플래그
    [SerializeField] private bool destroyOnPickup = true;
    //아이템을 주울 수 있는 대상 레이어 마스크
    [SerializeField] private LayerMask canBePickupBy;
    //아이템 주움 효과음
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 객체의 레이어가 canBePickupBy에 설정된 레이어 마스크와 일치하는지 확인
        if (canBePickupBy.value == (canBePickupBy.value | (1 << other.gameObject.layer)))
        {
            OnPickedUp(other.gameObject);

            //해당 효과음 재생
            if (pickupSound)
                SoundManager.PlayClip(pickupSound);
            // destroyOnPickup이 true로 설정되어 있으면 아이템 게임 오브젝트 파괴
            if (destroyOnPickup)
            {         
                Destroy(gameObject);
            }
        }
    }
    // 이 메서드는 추상 메서드로, 파생 클래스에서 반드시 구현해야 함
    protected abstract void OnPickedUp(GameObject receiver);
}
