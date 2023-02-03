using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour, IObjectPooler
{
    public Transform poolRoot;
    private GameObject _objectPrefab;
    private int _initialSize;
    private int _curSize;
    private Queue<GameObject> _availableObjects;
    
    public void Create(GameObject pf, int size)
    {
        _objectPrefab = pf;
        _initialSize = size;
        _curSize = _initialSize;
        _availableObjects = new Queue<GameObject>();
        
        for (int i = 0; i < _initialSize; i++)
        {
            ResetObject();
        }
    }
    
    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (NeedExpand())
        {
            ExpandPool();
            Dev.Log("Expand original pool attached to object " + gameObject.name + "; current size is " + _curSize);
        }
        
        GameObject obj = _availableObjects.Dequeue();
        // set transform
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        if (parent != null)
        {
            obj.transform.SetParent(parent);
        }
        //obj.SetActive(true);

        IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn(this);
        }
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        ResetObject(obj);
    }

    private bool NeedExpand()
    {
        return _availableObjects.Count == 0;
    }

    private void ExpandPool()
    {
        ResetObject();
        
        _curSize++;
    }

    private void ResetObject(GameObject obj = null)
    {
        if (obj == null)
        {
            obj = Instantiate(_objectPrefab, poolRoot);
        }
        obj.transform.SetParent(Core.Ins.transform);
        obj.SetActive(false);
        
        _availableObjects.Enqueue(obj);
    }
}