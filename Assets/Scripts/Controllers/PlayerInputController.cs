using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

// �Է��� �޾� ĳ���͸� ����
public class PlayerInputController : TopDownCharacterController
{
    private Camera _camera;

    private bool isMenu;
    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;

        isMenu = true;
    }

    public void OnMove(InputValue value)
    {
        if (!isMenu)
        {
            //Debug.Log("OnMove" + value.ToString()); 
            Vector2 moveInput = value.Get<Vector2>().normalized;
            CallMoveEvent(moveInput);
        }
    }

    public void OnLook(InputValue value)
    {
        if (!isMenu)
        {
            //Debug.Log("OnLook" + value.ToString());
            Vector2 newAim = value.Get<Vector2>();
            //��ũ�� ��ǥ�� ���� ��ǥ�� ���� ������� �Ѵ� 
            Vector2 worldPos = _camera.ScreenToWorldPoint(newAim);
            newAim = (worldPos - (Vector2)transform.position).normalized;

            //�ü� ������ ũ�Ⱑ ���� �� �̻��� ��� �� ���� �ٶ�
            if (newAim.magnitude >= .9f)
            {
                CallLookEvent(newAim);
            }
        }
    }

    //�Է°��� ���� ���� ������ ó��
    public void OnFire(InputValue value)
    {
        if (!isMenu)
        {
            //Debug.Log("OnFire" + value.ToString());
            IsAttacking = value.isPressed;
        }
    }

    //�޴� �Է�
    public void OnMenu(InputValue value)
    {
        isMenu = !isMenu;
        GameManager.instance.DisplayMenu(isMenu);
        Time.timeScale = isMenu ? 0 : 1;
    }
}
