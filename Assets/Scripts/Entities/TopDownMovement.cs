using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터의 움직임과 넉백을 관리하는 클래스
public class TopDownMovement : MonoBehaviour
{
    private TopDownCharacterController _controller;
    private CharacterStatsHandler _stats; 

    private Vector2 _movementDirection = Vector2.zero; 
    private Rigidbody2D _rigidbody;

    private Vector2 _knockback = Vector2.zero; //넉백 방향과 세기
    private float knockbackDuration = 0.0f; //넉백 지속시간
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
            //넉백 지속시간 줄임
            knockbackDuration -= Time.fixedDeltaTime; 
        }
    }
    private void Move(Vector2 direction)
    {
        _movementDirection = direction;
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        //넉백 지속시간 설정
        knockbackDuration = duration; 
        
        //넉백 방향과 세기 설정
        _knockback = -(other.position - transform.position).normalized * power; 
    }

    private void ApplyMovement(Vector2 direction)
    {
        direction = direction * _stats.CurrentStats.speed;

        // 넉백 중일 경우, 넉백 방향과 세기를 움직임 방향에 추가
        if (knockbackDuration >0.0f)
        {
            direction += _knockback; 
        }

        //가속도
        _rigidbody.velocity = direction; 
    }
}
