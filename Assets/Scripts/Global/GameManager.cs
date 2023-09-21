using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

//게임의 핵심 로직을 관리하고 게임의 진행, 웨이브 관리, 플레이어 상태, 로직의 업그레이드 및 보상생성등을 관리  
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform Player { get; private set;  }
    [SerializeField] private string playerTag = "Player";
    private HealthSystem playerHealthSystem;
    private CharacterStatsHandler playerurentStats; 
    private PlayerData playerData; 

    [SerializeField] private TextMeshProUGUI waveText; 
    [SerializeField] private Slider hpGaugeSlider;
    [SerializeField] private GameObject gameOverUI;

    private CharacterMenuUI characterMenuUI;

    [SerializeField] private int currentWaveIndex = 0;
    private int currentSpawnCount = 0; //현재 스폰된 적의 수 
    private int waveSpawnCount = 0; // 웨이브당 스폰되는 적의 수 
    private int waveSpawnPosCount = 0; //웨이브당 스폰 위치의 수 

    public float spawnInterval = .5f; // 스폰 간격
    public List<GameObject> enemyPrefebs = new List<GameObject>();//적 프리팹 목록

    [SerializeField] private Transform spawnPositionsRoot; // 적 스폰 위치를 가지고 있는 부모 위치
    private List<Transform> spawnPostions = new List<Transform>(); //적 스폰 위치 목록

    //보상 아이템 목록
    public List<GameObject> rewards = new List<GameObject>(); 
    public List<GameObject> equipRewards = new List<GameObject>();

    //기본 스탯
    [SerializeField] private CharacterStats defaultStats;

    //원거리 스탯
    [SerializeField] private CharacterStats rangedStats;


    private void Awake()
    {
        instance = this;
        Player = GameObject.FindGameObjectWithTag(playerTag).transform;

        playerHealthSystem = Player.GetComponent<HealthSystem>();
        playerHealthSystem.OnDamage += UpdateHealthUI;
        playerHealthSystem.OnHeal += UpdateHealthUI;
        playerHealthSystem.OnDeath += GameOver;

        playerData = Player.GetComponent<PlayerData>();
        playerurentStats = Player.GetComponent<CharacterStatsHandler>(); 

        gameOverUI.SetActive(false);
        characterMenuUI = UIManager.instance.GetUIComponent<CharacterMenuUI>();
        characterMenuUI.gameObject.SetActive(true);

        Time.timeScale = 0; 

        //스폰 위치를 리스트에 추가
        for (int i = 0; i < spawnPositionsRoot.childCount; i++)
        {
            spawnPostions.Add(spawnPositionsRoot.GetChild(i));
        }
    }

    private void Start()
    {
        //업그레이드 스탯 초기화 
        UpgradeStatInit();
        UpdateCharacterInfo();
       
        //다음 웨이브 시작을 위한 코루틴 시작
        StartCoroutine("StartNextWave");
    }

    private void Update()
    {
        UpdateAbility();
    }

    //다음 웨이브 시작
    IEnumerator StartNextWave()
    {
        while (true)
        {
            if (currentSpawnCount == 0)
            {
                //WaveUI 업데이트
                UpdateWaveUI();
                //2초대기
                yield return new WaitForSeconds(2f);

                if (currentWaveIndex % 20 == 0)
                {
                    // 무작위 업그레이드 적용
                    RandomUpgrade();
                }

                if (currentWaveIndex % 10 == 0)
                {
                    waveSpawnPosCount = waveSpawnPosCount + 1 > spawnPostions.Count ? waveSpawnPosCount : waveSpawnPosCount + 1;
                    waveSpawnCount = 0;
                    CreateEquipReward(); 
                }

                if (currentWaveIndex % 5 == 0)
                {
                    CreateReward();
                }

                if (currentWaveIndex % 3 == 0)
                {
                    waveSpawnCount += 1;
                }

                for (int i = 0; i < waveSpawnPosCount; i++)
                {
                    int posIdx = Random.Range(0, spawnPostions.Count);
                    for (int j = 0; j < waveSpawnCount; j++)
                    {
                        int prefabIdx = Random.Range(0, enemyPrefebs.Count);
                        GameObject enemy = Instantiate(enemyPrefebs[prefabIdx], spawnPostions[posIdx].position, Quaternion.identity);
                        enemy.GetComponent<HealthSystem>().OnDeath += OnEnemyDeath;
                        enemy.GetComponent<CharacterStatsHandler>().AddStatModifier(defaultStats);
                        enemy.GetComponent<CharacterStatsHandler>().AddStatModifier(rangedStats);
                        currentSpawnCount++;
                        yield return new WaitForSeconds(spawnInterval);
                    }
                }

                currentWaveIndex++;
            }

            yield return null;
        }
    }

    //적이 사망하면 스폰된 적 수를 줄임
    private void OnEnemyDeath()
    {
        currentSpawnCount--; 
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        StopAllCoroutines();
        gameOverUI.SetActive(true);
    }

    //healthBar 업데이트
    private void UpdateHealthUI()
    {
        hpGaugeSlider.value = playerHealthSystem.CurrentHealth / playerHealthSystem.MaxHealth; 
    }

    //wave 텍스트 업데이트
    private void UpdateWaveUI()
    {
        waveText.text = (currentWaveIndex + 1).ToString(); 
    }

    //메뉴화면
    public void DisplayMenu(bool isMenu)
    {
        characterMenuUI.gameObject.SetActive(isMenu); 
    }

    //캐릭터 정보 업데이트
    private void UpdateCharacterInfo()
    {
        characterMenuUI.SetCharacterInfo(playerData.playerName, playerData.jop, playerData.level, playerData.description, playerData.experience);
    }

    //캐릭터 능력치 업데이트
    private void UpdateAbility()
    {
        RangedAttackData curentPlayerAttacks = (RangedAttackData)playerurentStats.CurrentStats.attackSO;

        characterMenuUI.SetAbility((int)playerHealthSystem.MaxHealth, (int)curentPlayerAttacks.power, curentPlayerAttacks.numberOfProjectilesPershot, (int)playerurentStats.CurrentStats.speed, (int)curentPlayerAttacks.speed); 
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

    void CreateReward()
    {
        int idx = Random.Range(0, rewards.Count);
        int posIdx = Random.Range(0, spawnPostions.Count);

        GameObject obj = rewards[idx];
        Instantiate(obj, spawnPostions[posIdx].position, Quaternion.identity); 
    }

    void CreateEquipReward()
    {
        int idx = Random.Range(0, equipRewards.Count);
        int posIdx = Random.Range(0, spawnPostions.Count);

        GameObject obj = equipRewards[idx];
        Instantiate(obj, spawnPostions[posIdx].position, Quaternion.identity);
    }

    void UpgradeStatInit()
    {
        defaultStats.statsChangeType = StatsChangeType.Add;
        defaultStats.attackSO = Instantiate(defaultStats.attackSO);

        rangedStats.statsChangeType = StatsChangeType.Add;
        rangedStats.attackSO = Instantiate(rangedStats.attackSO);
    }

    void RandomUpgrade()
    {
        switch (Random.Range(0, 6))
        {
            case 0:
                defaultStats.maxHealth += 2; 
                break;
            case 1:
                defaultStats.attackSO.power += 1; 
                break;
            case 2:
                defaultStats.speed += 0.1f; 
                break;
            case 3:
                defaultStats.attackSO.isOnKnockback = true;
                defaultStats.attackSO.knockbackPower += 1;
                defaultStats.attackSO.knockbackTime = 0.1f; 
                break;
            case 4:
                defaultStats.attackSO.delay -= 0.05f; 
                break;
            case 5:
                RangedAttackData rangedAttackData = rangedStats.attackSO as RangedAttackData;
                rangedAttackData.numberOfProjectilesPershot += 1;
                break;
            default: 
                break; 
        }
    }
}
