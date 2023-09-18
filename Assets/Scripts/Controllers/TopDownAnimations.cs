using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터의 애니메이션 참조 할당
public class TopDownAnimations : MonoBehaviour
{
    protected Animator animator;
    protected TopDownCharacterController controller;

    protected virtual void Awake()
    {
        // 하위 게임 오브젝트 중에서 애니메이터 컴포넌트를 찾아 animator 변수에 할당
        animator = GetComponentInChildren<Animator>();

        // 현재 게임 오브젝트에 연결된 TopDownCharacterController 스크립트를 참조하여 controller 변수에 할당
        controller = GetComponent<TopDownCharacterController>(); 
    }
}
