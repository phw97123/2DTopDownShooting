using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ӿ��� �ֿ� �� �ִ� �������� ó���ϱ� ���� �⺻ Ʋ
public abstract class PickupItem : MonoBehaviour
{
    public ItemData item;
    //�������� �ֿ��� �� �ı����� ���θ� �����ϴ� �÷���
    [SerializeField] private bool destroyOnPickup = true;
    //�������� �ֿ� �� �ִ� ��� ���̾� ����ũ
    [SerializeField] private LayerMask canBePickupBy;
    //������ �ֿ� ȿ����
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ��ü�� ���̾ canBePickupBy�� ������ ���̾� ����ũ�� ��ġ�ϴ��� Ȯ��
        if (canBePickupBy.value == (canBePickupBy.value | (1 << other.gameObject.layer)))
        {
            OnPickedUp(other.gameObject);

            //�ش� ȿ���� ���
            if (pickupSound)
                SoundManager.PlayClip(pickupSound);
            // destroyOnPickup�� true�� �����Ǿ� ������ ������ ���� ������Ʈ �ı�
            if (destroyOnPickup)
            {         
                Destroy(gameObject);
            }
        }
    }
    // �� �޼���� �߻� �޼����, �Ļ� Ŭ�������� �ݵ�� �����ؾ� ��
    protected abstract void OnPickedUp(GameObject receiver);
}
