using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class TooltipController : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Tooltip tooltip;
    ItemSlotUI itemSlotUI;

    public void Start()
    {
        itemSlotUI = GetComponent<ItemSlotUI>(); 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSlotUI.curSlot != null)
        {
            ItemData item = GetComponent<ItemSlotUI>().curSlot.item;

            if (item != null)
            {
                tooltip.gameObject.SetActive(true);
                tooltip.SetupTooltip(item.displayName, item.description);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }
}
