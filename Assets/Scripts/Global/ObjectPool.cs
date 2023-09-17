using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable] //구조체를 Inspector 창에 노출시켜 수정 가능하게 만들기 위해 사용
    public struct Pool
    {
        public string tag;
        public GameObject prefab;
        public int size; 
    }

    public List<Pool> pools; //여러 풀을 관리하는 리스트
    public Dictionary<string, Queue<GameObject>> poolDictionary; //태그를 기반으로 풀을 관리하는 딕셔너리

    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>(); 
        foreach(var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>(); 

            //풀의 크기만큼 게임 오브젝트를 생성하고 비활성화 상태로 큐에 추가 
            for(int i = 0; i<pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj); 
            }
            poolDictionary.Add(pool.tag, objectPool); 
        }
    }

    //지정된 태그의 오브젝트를 오브젝트 풀에서 가져옴
    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
            return null;

        //오브젝트 풀에서 오브젝트를 꺼내고 다시 큐에 추가하여 재사용
        GameObject obj = poolDictionary[tag].Dequeue();
        poolDictionary[tag].Enqueue(obj);
        return obj; 
    }
}
