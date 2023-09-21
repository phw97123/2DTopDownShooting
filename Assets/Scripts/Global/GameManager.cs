using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

//������ �ٽ� ������ �����ϰ� ������ ����, ���̺� ����, �÷��̾� ����, ������ ���׷��̵� �� ����������� ����  
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
    private int currentSpawnCount = 0; //���� ������ ���� �� 
    private int waveSpawnCount = 0; // ���̺�� �����Ǵ� ���� �� 
    private int waveSpawnPosCount = 0; //���̺�� ���� ��ġ�� �� 

    public float spawnInterval = .5f; // ���� ����
    public List<GameObject> enemyPrefebs = new List<GameObject>();//�� ������ ���

    [SerializeField] private Transform spawnPositionsRoot; // �� ���� ��ġ�� ������ �ִ� �θ� ��ġ
    private List<Transform> spawnPostions = new List<Transform>(); //�� ���� ��ġ ���

    //���� ������ ���
    public List<GameObject> rewards = new List<GameObject>(); 
    public List<GameObject> equipRewards = new List<GameObject>();

    //�⺻ ����
    [SerializeField] private CharacterStats defaultStats;

    //���Ÿ� ����
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

        //���� ��ġ�� ����Ʈ�� �߰�
        for (int i = 0; i < spawnPositionsRoot.childCount; i++)
        {
            spawnPostions.Add(spawnPositionsRoot.GetChild(i));
        }
    }

    private void Start()
    {
        //���׷��̵� ���� �ʱ�ȭ 
        UpgradeStatInit();
        UpdateCharacterInfo();
       
        //���� ���̺� ������ ���� �ڷ�ƾ ����
        StartCoroutine("StartNextWave");
    }

    private void Update()
    {
        UpdateAbility();
    }

    //���� ���̺� ����
    IEnumerator StartNextWave()
    {
        while (true)
        {
            if (currentSpawnCount == 0)
            {
                //WaveUI ������Ʈ
                UpdateWaveUI();
                //2�ʴ��
                yield return new WaitForSeconds(2f);

                if (currentWaveIndex % 20 == 0)
                {
                    // ������ ���׷��̵� ����
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

    //���� ����ϸ� ������ �� ���� ����
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

    //healthBar ������Ʈ
    private void UpdateHealthUI()
    {
        hpGaugeSlider.value = playerHealthSystem.CurrentHealth / playerHealthSystem.MaxHealth; 
    }

    //wave �ؽ�Ʈ ������Ʈ
    private void UpdateWaveUI()
    {
        waveText.text = (currentWaveIndex + 1).ToString(); 
    }

    //�޴�ȭ��
    public void DisplayMenu(bool isMenu)
    {
        characterMenuUI.gameObject.SetActive(isMenu); 
    }

    //ĳ���� ���� ������Ʈ
    private void UpdateCharacterInfo()
    {
        characterMenuUI.SetCharacterInfo(playerData.playerName, playerData.jop, playerData.level, playerData.description, playerData.experience);
    }

    //ĳ���� �ɷ�ġ ������Ʈ
    private void UpdateAbility()
    {
        RangedAttackData curentPlayerAttacks = (RangedAttackData)playerurentStats.CurrentStats.attackSO;

        characterMenuUI.SetAbility((int)playerHealthSystem.MaxHealth, (int)curentPlayerAttacks.power, curentPlayerAttacks.numberOfProjectilesPershot, (int)playerurentStats.CurrentStats.speed, (int)curentPlayerAttacks.speed); 
    }

    public void RestartGame()
    {
        //���� �����ִ� ���� ��ȣ�� �����ͼ� �� ���� �ٽ� �ε��Ѵ�
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
