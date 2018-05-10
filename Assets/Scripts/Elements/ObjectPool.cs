using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    List<GameObject> pooledStuff = new List<GameObject>();
    private GameObject prefabGen;
    
    public void Initialize(int size, GameObject prefab)
    {
        prefabGen = prefab;
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate<GameObject>(prefabGen);
            obj.SetActive(false);
            pooledStuff.Add(obj);
            obj.transform.SetParent(gameObject.transform);
        }
    }

    public GameObject GetObject()
    {
        GameObject obj;
        if (pooledStuff.Count > 0)
        {
            obj = pooledStuff[0];
            pooledStuff.RemoveAt(0);
            return obj;
        }
        obj = Instantiate<GameObject>(prefabGen);
        obj.transform.SetParent(gameObject.transform);
        return obj; 
    }

    public void DestroyObjectPool(GameObject obj)
    {
        pooledStuff.Add(obj);
        obj.SetActive(false);
    }


    public void ClearPool()
    {
        for (int i = pooledStuff.Count - 1; i > 0; i--)
        {
            GameObject obj = pooledStuff[i];
            pooledStuff.RemoveAt(i);
            Destroy(obj);
        }
        pooledStuff = null;
    }
}
