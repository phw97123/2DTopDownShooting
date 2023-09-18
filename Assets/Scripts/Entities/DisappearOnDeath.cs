using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

//ĳ���Ͱ� ����� �� Ư�� ������ �����ϵ��� �ϴ� Ŭ����
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
        //�������� ���ϰ� �ڸ� ����
        _rigidbody.velocity = Vector3.zero; 

        //���� �����ؼ� ������ �ִ� ��� ��������Ʈ�� ã�´� 
        foreach(SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            //�����ϰ� ����
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color; 
        }

        //Behaviour�� MonoBehaviour�� ���� �θ��̴� 
        foreach(Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            //����ϴ� ��ũ��Ʈ���� ����
            component.enabled = false;
        }
        //2���Ŀ� �ı�
        Destroy(gameObject, 2f);
    }
}
