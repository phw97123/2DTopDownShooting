using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

//캐릭터 메뉴 UI를 관리하는 클래스
public class CharacterMenuUI : MonoBehaviour
{
    //BtnPanel
    [SerializeField] private Button infoBtn;
    [SerializeField] private Button abilityBtn;
    [SerializeField] private Button inventoryBtn;

    //InfoPanel
    [SerializeField] private GameObject infoPanel; 
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text jopText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Slider levelSlider;

    //AbilityPanel 
    [SerializeField] private GameObject abilityPanel;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text powerText;
    [SerializeField] private TMP_Text projectileCountText;
    [SerializeField] private TMP_Text SpeedText;

    [SerializeField] private GameObject inventoryPanel;

    [SerializeField] private GameObject isEquipPanel;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private TextMeshProUGUI isEquipText; 
    
    private void Awake()
    {
        abilityPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        isEquipPanel.SetActive(false);
    }

    private void Start()
    {
        if(infoBtn != null)
        {
            infoBtn.onClick.AddListener(DisplayInfo); 
        }

        if(infoBtn != null )
        {
            abilityBtn.onClick.AddListener(DisplayAbility);
        }

        if (infoBtn != null)
        {
            inventoryBtn.onClick.AddListener(DisplayInventory);
        }

        if (infoBtn != null)
        {
            cancelBtn.onClick.AddListener(OffIsEquipPanel);
        }

        if (infoBtn != null)
        {
            confirmBtn.onClick.AddListener(OnConfirmButton);
        }
    }

    //캐릭터 정보 패널 
    private void DisplayInfo()
    {
        infoPanel.SetActive(true);
        abilityPanel.SetActive(false);
        inventoryPanel.SetActive(false); 
    }

    //캐릭터 정보 입력받을 함수
    public void SetCharacterInfo(string playerName, string job, int level, string description, float exp)
    {
        playerNameText.text = playerName;
        jopText.text = job;
        levelText.text = "Level " + level.ToString();
        descriptionText.text = description;

        levelSlider.value = exp / 100f;
    }

    //능력치 패널
    private void DisplayAbility()
    {
        abilityPanel.SetActive(true);
        inventoryPanel.SetActive(false); 
        infoPanel.SetActive(false);
    }

    //능력치 정보 입력받을 함수
    public void SetAbility(int hp, int power, int projectileCount, int speed)
    {
        hpText.text = hp.ToString();
        powerText.text = power.ToString();
        projectileCountText.text = projectileCount.ToString();
        SpeedText.text = speed.ToString(); 


    }

    private void DisplayInventory()
    {
        abilityPanel.SetActive(false);
        inventoryPanel.SetActive(true);
        infoPanel.SetActive(false);
    }

    public void OnIsEquipPanel()
    {
        isEquipPanel.SetActive(true); 
    }

    public void OnConfirmButton()
    {
        if(Inventory.instance != null)
        {
            Inventory.instance.OnConfirmButtonPressed?.Invoke(); 
        }

        OffIsEquipPanel(); 
    }

    private void OffIsEquipPanel()
    {
        isEquipPanel.SetActive(false);
    }

    public void SetIsEquipText(bool isEquip)
    {
        if(!isEquip)
        {
            isEquipText.text = "장착 하시겠습니까 ?"; 
        }
        else
            isEquipText.text = "장착을 해제 하시겠습니까 ?";
    }
}
