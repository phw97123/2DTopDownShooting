using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//스탯 변경 유형을 정의하는 열거형
public enum StatsChangeType
{
    Add, //스탯을 추가
    Multiple, //스탯을 곱셈하여 적용
    Override, //스탯을 덮어쓴다
}

[Serializable]
//캐릭터의 스탯과 공격 데이터를 정의
public class CharacterStats 
{
    //스탯 변경 유형
    public StatsChangeType statsChangeType; 

    //최대 최력 값
    [Range(1, 100)] public int maxHealth;

    //이동 속도값
    [Range(1f, 20f)] public float speed;

    //공격 데이터
    public AttackSO attackSO; 

}
