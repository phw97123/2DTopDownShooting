using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ĳ������ �����Ӱ� �˹��� �����ϴ� Ŭ����
public class TopDownMovement : MonoBehaviour
{
    private TopDownCharacterController _controller;
    private CharacterStatsHandler _stats; 

    private Vector2 _movementDirection = Vector2.zero; 
    private Rigidbody2D _rigidbody;

    private Vector2 _knockback = Vector2.zero; //�˹� ����� ����
    private float knockbackDuration = 0.0f; //�˹� ���ӽð�
    private void Awake()
    {
        _controller = GetComponent<TopDownCharacterController>();
        _stats = GetComponent<CharacterStatsHandler>(); 
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        _controller.OnMoveEvent += Move; 
    }

    private void FixedUpdate()
    {
        ApplyMovement(_movementDirection); 
        if(knockbackDuration >0.0f)
        {
            //�˹� ���ӽð� ����
            knockbackDuration -= Time.fixedDeltaTime; 
        }
    }
    private void Move(Vector2 direction)
    {
        _movementDirection = direction;
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        //�˹� ���ӽð� ����
        knockbackDuration = duration; 
        
        //�˹� ����� ���� ����
        _knockback = -(other.position - transform.position).normalized * power; 
    }

    private void ApplyMovement(Vector2 direction)
    {
        direction = direction * _stats.CurrentStats.speed;

        // �˹� ���� ���, �˹� ����� ���⸦ ������ ���⿡ �߰�
        if (knockbackDuration >0.0f)
        {
            direction += _knockback; 
        }

        //���ӵ�
        _rigidbody.velocity = direction; 
    }
}
