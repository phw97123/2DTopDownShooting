using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownAimRotation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private Transform armPivot;

    [SerializeField] private SpriteRenderer characterRenderer;

    private TopDownCharacterController _contorller;

    private void Awake()
    {
        _contorller = GetComponent<TopDownCharacterController>(); 
    }
    // Start is called before the first frame update
    void Start()
    {
        _contorller.OnLookEvent += OnAim; 
    }

    public void OnAim(Vector2 newAimDirection)
    {
        RotateArm(newAimDirection); 
    }

    private void RotateArm(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        armRenderer.flipY = Mathf.Abs(rotZ) > 90f;
        characterRenderer.flipX = armRenderer.flipY;
        armPivot.rotation = Quaternion.Euler(0, 0, rotZ); 
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
