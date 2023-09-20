using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Button button;
    public Image icon;
    public Image equipImag; 
    public ItemSlot slot;
    public ItemSlot curSlot; 
    private Outline outline;

    private CharacterStats statsModifier; 

    public int index;
    public bool equipped;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        equipImag.gameObject.SetActive(false);
        slot = new ItemSlot();
        curSlot = new ItemSlot();
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Set(ItemSlot slot)
    {
        curSlot = slot;
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.sprite;
        statsModifier = slot.statsModifier; 

        if (outline != null)
        {
            outline.enabled = equipped; 
        }
    }

    public void Clear()
    {
        curSlot = null;
        icon.gameObject.SetActive(false);
        equipImag.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        OnEnable();
        Inventory.instance.SelectItem(index);
    }

    public void SetEquipImage(bool isEquip)
    {
        equipImag.gameObject.SetActive(isEquip);
    }
}
