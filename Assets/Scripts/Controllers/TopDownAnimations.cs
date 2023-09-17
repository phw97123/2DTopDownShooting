using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownAnimations : MonoBehaviour
{
    protected Animator animator;
    protected TopDownCharacterController controller;

    protected virtual void Awake()
    {
        // 하위에 애니메이터 생성
        animator = GetComponentInChildren<Animator>(); 
        controller = GetComponent<TopDownCharacterController>(); 
    }
}
