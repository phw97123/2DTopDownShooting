using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ĳ������ �ִϸ��̼� ���� �Ҵ�
public class TopDownAnimations : MonoBehaviour
{
    protected Animator animator;
    protected TopDownCharacterController controller;

    protected virtual void Awake()
    {
        // ���� ���� ������Ʈ �߿��� �ִϸ����� ������Ʈ�� ã�� animator ������ �Ҵ�
        animator = GetComponentInChildren<Animator>();

        // ���� ���� ������Ʈ�� ����� TopDownCharacterController ��ũ��Ʈ�� �����Ͽ� controller ������ �Ҵ�
        controller = GetComponent<TopDownCharacterController>(); 
    }
}
