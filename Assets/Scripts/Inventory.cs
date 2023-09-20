using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ItemSlot
{
    public ItemData item;
    public CharacterStats statsModifier; 
}

public class Inventory : MonoBehaviour
{
    //확인 버튼 눌렸을 때 
    public Action OnConfirmButtonPressed; 

    //player 능력치
    private CharacterStatsHandler playerStatsHandler; 

    public ItemSlotUI[] uiSlots;
    public ItemSlot[] slots;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;

    private int curEquipIndex = -1;

    public static Inventory instance;

    private CharacterMenuUI menuUI; 

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerStatsHandler = GameManager.instance.Player.GetComponent< CharacterStatsHandler>(); 

        menuUI = UIManager.instance.GetUIComponent<CharacterMenuUI>();
        OnConfirmButtonPressed += HandleConfirmButtonPressed;

        slots = new ItemSlot[uiSlots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
            uiSlots[i].index = i;
            uiSlots[i].Clear();
        }

        selectedItem = null;
    }

    public void AddItem(ItemData item, CharacterStats statsModifier)
    {
        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.statsModifier = statsModifier; 
            UpdateUI();
        }
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }
        return null;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                uiSlots[i].Set(slots[i]);
            else
                uiSlots[i].Clear();

            if(curEquipIndex >= 0 && curEquipIndex<uiSlots.Length)
            {
                //장착 이미지
                if (uiSlots[curEquipIndex].equipped)
                    uiSlots[curEquipIndex].SetEquipImage(true);
                else
                    uiSlots[curEquipIndex].SetEquipImage(false);
            }
        }
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null)
            return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        menuUI.OnIsEquipPanel();

        if (!uiSlots[selectedItemIndex].equipped)
            menuUI.SetIsEquipText(false); // 장착하지 않은 아이템을 누를 때
        else
            menuUI.SetIsEquipText(true); // 이미 장착된 아이템을 누를 때

        menuUI.OnIsEquipPanel();
    }

    private void HandleConfirmButtonPressed()
    {
        if (curEquipIndex != -1 && uiSlots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }

        if(curEquipIndex != selectedItemIndex)
        {
            uiSlots[selectedItemIndex].equipped = true;
            curEquipIndex = selectedItemIndex;
            playerStatsHandler.AddStatModifier(slots[curEquipIndex].statsModifier);
            UpdateUI();
        }
        else
        {
            UnEquip(curEquipIndex);
            curEquipIndex = -1; 
        }
        SelectItem(selectedItemIndex);
    }

    private void UnEquip(int index)
    {
        uiSlots[index].equipped = false;
        UpdateUI();
        playerStatsHandler.RemoveStatModifier(slots[index].statsModifier); 

        if (selectedItemIndex == index)
            SelectItem(index); 
    }
}
