using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//IPointerEnterHandler
//이 인터페이스를 구현한 클래스는 해당 UI 요소 위에 마우스 포인터가 진입할 때 호출되는 OnPointerEnter 메서드를 구현 해야함
//IPointerExitHandler:
//이 인터페이스를 구현한 클래스는 해당 UI 요소에서 마우스 포인터가 빠져나갈 때 호출되는 OnPointerExit 메서드를 구현해야함 

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
