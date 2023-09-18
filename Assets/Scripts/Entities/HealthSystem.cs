using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터의 생명력을 관리하고, 데미지를 입히거나 회복시키는데 사용
public class HealthSystem : MonoBehaviour
{
    //체력 변경 후의 딜레이 시간 설정
    [SerializeField] private float healthChangeDelay = .5f;

    private CharacterStatsHandler _statsHandler;

    //마지막 체력 변경 이후의 경과 시간
    private float _timeSinceLastChange = float.MaxValue;

    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;

    //데미지를 받을 때 재생될 오디오 클립
    public AudioClip damageClip; 
    public float CurrentHealth { get; private set; }

    public float MaxHealth => _statsHandler.CurrentStats.maxHealth;

    private void Awake()
    {
        _statsHandler = GetComponent<CharacterStatsHandler>(); 
    }

    private void Start()
    {
        CurrentHealth = _statsHandler.CurrentStats.maxHealth; 
    }

    private void Update()
    {
        //체력 변경 후의 딜레이 시간을 관리
        if(_timeSinceLastChange <healthChangeDelay)
        {
            _timeSinceLastChange += Time.deltaTime; 
            if(_timeSinceLastChange >= healthChangeDelay)
            {
                OnInvincibilityEnd?.Invoke();  //무적 상태 종료
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        //변경량이 0이거나 체력 변경 후의 딜레이 시간 내에 변경되었으면 아무 작업도 하지 않는다
        if (change == 0 || _timeSinceLastChange < healthChangeDelay)
            return false;

        _timeSinceLastChange = 0f;
        CurrentHealth += change;
        //최대 체력을 초과하지 않도록 제한
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        //음수가 되지 않도록 제한
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        //변경량에 따라 데미지 또는 회복 이벤트 호출
        if (change > 0)
            OnHeal?.Invoke();
        else
        {
            OnDamage?.Invoke();
            if (damageClip)
                SoundManager.PlayClip(damageClip); 
        }
        // 체력이 0 이하로 떨어지면 사망 이벤트를 호출
        if (CurrentHealth<=0f)
        {
            CallDeath(); 
        }

        return true; 
    }

    private void CallDeath()
    {
        OnDeath?.Invoke(); 
    }
}
