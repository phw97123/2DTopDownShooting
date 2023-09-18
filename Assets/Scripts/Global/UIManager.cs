using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Dictionary<string, MonoBehaviour> uiComponentDic;

    private void Awake()
    {
        instance = this;
        uiComponentDic = new Dictionary<string, MonoBehaviour>(); 
    }

    public T GetUIComponent<T>() where T : MonoBehaviour
    {
        if (!uiComponentDic.ContainsKey(typeof(T).Name))
        {
            var obj = Instantiate(Resources.Load($"Prefabs/{typeof(T).Name}"));
            uiComponentDic.Add(typeof(T).Name, obj.GetComponent<T>()); 
        }
        return uiComponentDic[typeof(T).Name] as T; 
    }
}
