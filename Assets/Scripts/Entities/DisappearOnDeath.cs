using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class DisappearOnDeath : MonoBehaviour
{
    private HealthSystem _healthSystem;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _healthSystem.OnDeath += OnDeath;
    }

    void OnDeath()
    {
        //움직이지 못하게 자리 고정
        _rigidbody.velocity = Vector3.zero; 

        //나를 포함해서 하위에 있는 모든 스프라이트를 찾는다 
        foreach(SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            //투명하게 만듬
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color; 
        }

        //Behaviour는 MonoBehaviour의 위의 부모이다 
        foreach(Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            //사용하는 스크립트들을 끈다
            component.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
}
