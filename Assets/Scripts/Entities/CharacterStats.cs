using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���� ���� ������ �����ϴ� ������
public enum StatsChangeType
{
    Add, //������ �߰�
    Multiple, //������ �����Ͽ� ����
    Override, //������ �����
}

[Serializable]
//ĳ������ ���Ȱ� ���� �����͸� ����
public class CharacterStats 
{
    //���� ���� ����
    public StatsChangeType statsChangeType; 

    //�ִ� �ַ� ��
    [Range(1, 100)] public int maxHealth;

    //�̵� �ӵ���
    [Range(1f, 20f)] public float speed;

    //���� ������
    public AttackSO attackSO; 

}
