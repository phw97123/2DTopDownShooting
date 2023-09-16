using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action OnAttackEvent;

    private float _timeSinceLastAttack = float.MaxValue; 
    protected bool IsAttacking { get; set; }

    protected CharacterStatsHandler Stats { get; private set;  }

    protected virtual void Awake()
    {
        Stats = GetComponent<CharacterStatsHandler>(); 
    }

    protected virtual void Update()
    {
        HandleAttackDelay(); 
    }

    private void HandleAttackDelay()
    {
        if (Stats.CurrentStates.attackSO == null)
            return;

        if(_timeSinceLastAttack <= Stats.CurrentStates.attackSO.delay)
        {
            _timeSinceLastAttack += Time.deltaTime; 
        }

        if (IsAttacking && _timeSinceLastAttack > Stats.CurrentStates.attackSO.delay) 
        {
            _timeSinceLastAttack = 0;
            CallAttackEvnet(); 
        }
    }

    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction); 
    }
    public void CallLookEvent(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }
    public void CallAttackEvnet()
    {
        OnAttackEvent?.Invoke();
    }
}


//{
//    //간단한 속도 이동 방법 
//    //[SerializeField] private float speed = 5f; 

//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //간단한 이동 방법 
//        //float x = Input.GetAxisRaw("Horizontal");
//        //float y = Input.GetAxisRaw("Vertical");

//        //transform.position += new Vector3(x, y) * speed * Time.deltaTime;  
//    }
//}
