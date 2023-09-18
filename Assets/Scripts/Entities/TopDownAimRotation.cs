using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾� ĳ������ ���� ���⿡ ���� ĳ���� �Ȱ� ĳ���� �� ��ü�� ȸ����Ű�� Ŭ����
public class TopDownAimRotation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private Transform armPivot; // ���� ȸ���� �߽���

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

    // ���� ���⿡ ���� ���� ȸ����Ű�� �޼���
    private void RotateArm(Vector2 direction)
    {
        // ���� ������ ������ ��ȯ
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // �� ��������Ʈ�� Y ������ ���θ� ����
        armRenderer.flipY = Mathf.Abs(rotZ) > 90f;
        // ĳ���� ��������Ʈ�� X ������ ���θ� �Ȱ� �����ϰ� ����
        characterRenderer.flipX = armRenderer.flipY;
        // ���� ȸ�� �߽����� �̿��Ͽ� ���� ȸ��
        armPivot.rotation = Quaternion.Euler(0, 0, rotZ); 
    }
}
