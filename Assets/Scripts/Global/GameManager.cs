using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform Player { get; private set;  }
    [SerializeField] private string playerTag = "Player";
    private HealthSystem playerHealthSystem; 

    [SerializeField] private TextMeshProUGUI waveText; 
    [SerializeField] private Slider hpGaugeSlider;
    [SerializeField] private GameObject gameOverUI; 
    

    private void Awake()
    {
        instance = this;
        Player = GameObject.FindGameObjectWithTag(playerTag).transform;

        playerHealthSystem = Player.GetComponent<HealthSystem>();
        playerHealthSystem.OnDamage += UpdateHealthUI;
        playerHealthSystem.OnHeal += UpdateHealthUI;
        playerHealthSystem.OnDeath += GameOver;

        gameOverUI.SetActive(false); 
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true); 
    }

    private void UpdateHealthUI()
    {
        hpGaugeSlider.value = playerHealthSystem.CurrentHealth / playerHealthSystem.MaxHealth; 
    }

    private void UpdateWaveUI()
    {
        //waveText.text = 
    }

    public void RestartGame()
    {
        //지금 켜져있는 씬에 번호를 가져와서 그 씬을 다시 로드한다
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    public void ExitGame()
    {
        Application.Quit(); 
    }
}
