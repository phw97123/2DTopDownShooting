using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

// 입력을 받아 캐릭터를 제어
public class PlayerInputController : TopDownCharacterController
{
    private Camera _camera;

    protected override void Awake()
    {
        base.Awake(); 
        _camera = Camera.main;
    }

    public void OnMove(InputValue value)
    {
        //Debug.Log("OnMove" + value.ToString()); 
        Vector2 moveInput = value.Get<Vector2>().normalized; 
        CallMoveEvent(moveInput);
    }

    public void OnLook(InputValue value)
    {
        //Debug.Log("OnLook" + value.ToString());
        Vector2 newAim = value.Get<Vector2>();
        //스크린 좌표를 월드 좌표로 변경 시켜줘야 한다 
        Vector2 worldPos = _camera.ScreenToWorldPoint(newAim);
        newAim = (worldPos - (Vector2)transform.position).normalized; 

        //시선 벡터의 크기가 일정 값 이상인 경우 그 쪽을 바라봄
        if(newAim.magnitude >= .9f)
        {
            CallLookEvent(newAim);
        }
    }

    //입력값에 따라 공격 동작을 처리
    public void OnFire(InputValue value)
    {
        //Debug.Log("OnFire" + value.ToString());
        IsAttacking = value.isPressed; 
    }
}
