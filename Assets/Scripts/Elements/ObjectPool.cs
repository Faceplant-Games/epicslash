using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> _pooledStuff = new List<GameObject>();
    private GameObject _prefabGen;
    
    public void Initialize(int size, GameObject prefab)
    {
        _prefabGen = prefab;
        for (var i = 0; i < size; i++)
        {
            var obj = Instantiate(_prefabGen);
            obj.name = prefab.name;
            obj.SetActive(false);
            _pooledStuff.Add(obj);
            obj.transform.SetParent(gameObject.transform);
        }
    }

    public GameObject GetObject()
    {
        GameObject obj;
        if (_pooledStuff.Count > 0)
        {
            obj = _pooledStuff[0];
            _pooledStuff.RemoveAt(0);
            return obj;
        }
        obj = Instantiate(_prefabGen);
        obj.transform.SetParent(gameObject.transform);
        return obj; 
    }

    public void DestroyObjectPool(GameObject obj)
    {
        _pooledStuff.Add(obj);
        obj.SetActive(false);
    }

    public void ClearPool()
    {
        for (var i = _pooledStuff.Count - 1; i > 0; i--)
        {
            var obj = _pooledStuff[i];
            _pooledStuff.RemoveAt(i);
            Destroy(obj);
        }
        _pooledStuff = null;
    }
}
