using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerData : MonoBehaviour
{
    public string playerName = "Player";
    public string jop = "궁수"; 
    public int level = 1;
    public int experience = 0;
    public string description = "원거리 공격을 한다."; 
    public int gold = 0; 

    public void GainExperience(int amount)
    {
        experience += amount; 

        if(experience>= CalculateExperienceRequiredForNextLevel())
        {
            LevelUp(); 
        }
    }

    private void LevelUp()
    {
        level++;
        experience = 0; 
    }

    private int CalculateExperienceRequiredForNextLevel()
    {
        return level * 100; 
    }

    public void AddGold(int amount)
    {
        gold += amount; 
    }

    public void RemoveGold(int amount)
    {
        gold -= amount;

        if (gold < 0)
            gold = 0; 
    }

    public void SetPlayerName(string newName)
    {
        name = newName; 
    }

    public void SetDescription(string newDescription)
    {
        description = newDescription; 
    }
}
