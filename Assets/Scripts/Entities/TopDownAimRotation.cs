using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 캐릭터의 조준 방향에 따라 캐릭터 팔과 캐릭터 그 자체를 회전시키는 클래스
public class TopDownAimRotation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private Transform armPivot; // 팔의 회전의 중심점

    [SerializeField] private SpriteRenderer characterRenderer;

    private TopDownCharacterController _contorller;

    private void Awake()
    {
        _contorller = GetComponent<TopDownCharacterController>(); 
    }

    void Start()
    {
        _contorller.OnLookEvent += OnAim; 
    }

    public void OnAim(Vector2 newAimDirection)
    {
        RotateArm(newAimDirection); 
    }

    // 조준 방향에 따라 팔을 회전시키는 메서드
    private void RotateArm(Vector2 direction)
    {
        // 조준 방향을 각도로 변환
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 팔 스프라이트의 Y 뒤집기 여부를 설정
        armRenderer.flipY = Mathf.Abs(rotZ) > 90f;
        // 캐릭터 스프라이트의 X 뒤집기 여부를 팔과 동일하게 설정
        characterRenderer.flipX = armRenderer.flipY;
        // 팔의 회전 중심점을 이용하여 팔을 회전
        armPivot.rotation = Quaternion.Euler(0, 0, rotZ); 
    }
}
