using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ĳ������ ������� �����ϰ�, �������� �����ų� ȸ����Ű�µ� ���
public class HealthSystem : MonoBehaviour
{
    //ü�� ���� ���� ������ �ð� ����
    [SerializeField] private float healthChangeDelay = .5f;

    private CharacterStatsHandler _statsHandler;

    //������ ü�� ���� ������ ��� �ð�
    private float _timeSinceLastChange = float.MaxValue;

    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;

    //�������� ���� �� ����� ����� Ŭ��
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
        //ü�� ���� ���� ������ �ð��� ����
        if(_timeSinceLastChange <healthChangeDelay)
        {
            _timeSinceLastChange += Time.deltaTime; 
            if(_timeSinceLastChange >= healthChangeDelay)
            {
                OnInvincibilityEnd?.Invoke();  //���� ���� ����
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        //���淮�� 0�̰ų� ü�� ���� ���� ������ �ð� ���� ����Ǿ����� �ƹ� �۾��� ���� �ʴ´�
        if (change == 0 || _timeSinceLastChange < healthChangeDelay)
            return false;

        _timeSinceLastChange = 0f;
        CurrentHealth += change;
        //�ִ� ü���� �ʰ����� �ʵ��� ����
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        //������ ���� �ʵ��� ����
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        //���淮�� ���� ������ �Ǵ� ȸ�� �̺�Ʈ ȣ��
        if (change > 0)
            OnHeal?.Invoke();
        else
        {
            OnDamage?.Invoke();
            if (damageClip)
                SoundManager.PlayClip(damageClip); 
        }
        // ü���� 0 ���Ϸ� �������� ��� �̺�Ʈ�� ȣ��
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
