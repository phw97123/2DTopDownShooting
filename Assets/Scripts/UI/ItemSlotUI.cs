using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Button button;
    public Image icon;
    private ItemSlot slot;
    private ItemSlot curSlot; 
    private Outline outline;

    public int index;
    public bool equipped;

    private void Awake()
    {
        outline = GetComponent<Outline>(); 
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
        
        if(outline != null)
        {
            outline.enabled = equipped; 
        }
    }

    public void Clear()
    {
        curSlot = null;
        icon.gameObject.SetActive(false); 
    }

    public void OnButtonClick()
    {
        Inventory.instance.SelectItem(index);
    }

}
