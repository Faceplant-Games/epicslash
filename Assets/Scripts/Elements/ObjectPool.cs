using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    List<GameObject> pooledStuff;
    private GameObject prefabGen;
    
    public void Initialize(int size, GameObject prefab)
    {
        prefabGen = prefab;
        pooledStuff = new List<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate<GameObject>(prefabGen);
            obj.SetActive(false);
            pooledStuff.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        if (pooledStuff.Count > 0)
        {
            GameObject obj = pooledStuff[0];
            pooledStuff.RemoveAt(0);
            return obj;
        }
        return Instantiate<GameObject>(prefabGen); 
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
