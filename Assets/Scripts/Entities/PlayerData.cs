using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerData : MonoBehaviour
{
    public string playerName;
    public string jop; 
    public int level;
    public int experience;
    public string description; 
    public int gold; 

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
