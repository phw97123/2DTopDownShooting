using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    RectTransform rt; 
    private float halfwidth; 

    private void Start()
    {
        halfwidth = GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f;
        rt = GetComponent<RectTransform>(); 
    }

    private void Update()
    {
        transform.position = Input.mousePosition;

        //만약 UI 요소가 화면 오른쪽 반 이상에 위치하면 UI 요소의 pivot을 오른쪽 상단(1, 1)으로 설정
        if (rt.anchoredPosition.x + rt.sizeDelta.x + 90> halfwidth)
            rt.pivot = new Vector2(1, 1);
        //그렇지 않으면(UI 요소가 화면 왼쪽 반에 위치하면) UI 요소의 pivot을 왼쪽 상단(0, 1)으로 설정
        else
            rt.pivot = new Vector2(0, 1); 
    }

    public void SetupTooltip(string name, string des)
    {
        nameText.text = name;
        descriptionText.text = des;
    }
}
