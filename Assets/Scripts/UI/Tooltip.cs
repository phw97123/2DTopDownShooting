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

        //���� UI ��Ұ� ȭ�� ������ �� �̻� ��ġ�ϸ� UI ����� pivot�� ������ ���(1, 1)���� ����
        if (rt.anchoredPosition.x + rt.sizeDelta.x + 90> halfwidth)
            rt.pivot = new Vector2(1, 1);
        //�׷��� ������(UI ��Ұ� ȭ�� ���� �ݿ� ��ġ�ϸ�) UI ����� pivot�� ���� ���(0, 1)���� ����
        else
            rt.pivot = new Vector2(0, 1); 
    }

    public void SetupTooltip(string name, string des)
    {
        nameText.text = name;
        descriptionText.text = des;
    }
}
