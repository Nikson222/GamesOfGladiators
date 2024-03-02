using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPooler<T> where T : MonoBehaviour
{
    private T _prefab;
    private Transform _parent;
    private int _countOfSpawningPool = 20;
    private Queue<T> _queueOfPool = new Queue<T>();
    
    public ObjectPooler(int countSpawn)
    {
        _countOfSpawningPool = countSpawn;
    }
    public void Init(T poolObject, Transform parent)
    {
        SetPrefab(poolObject);
        _parent = parent;
        _queueOfPool.Clear();
    }
    public void Init(T poolObject)
    {
        SetPrefab(poolObject);
        _queueOfPool.Clear();
    }
    private void SetPrefab(T poolObject) => _prefab = poolObject;
    public T GetObject()
    {
        if (_queueOfPool.Count > 0)
        {
            for (int i = 0; i < _queueOfPool.Count; i++)
            {
                if (_queueOfPool.Peek().gameObject.activeInHierarchy.Equals(false))
                {
                    var poolObject = _queueOfPool.Dequeue();
                    poolObject.gameObject.SetActive(true);
                    _queueOfPool.Enqueue(poolObject);
                    return poolObject;
                }
                else
                {
                    var poolObject = _queueOfPool.Dequeue();
                    _queueOfPool.Enqueue(poolObject);
                }
            }
            return SpawnPool();
        }
        else
        {
            return SpawnPool();
        }
    }
    
    public void ReturnObject(T returnedObject)
    {
        returnedObject.gameObject.SetActive(false);
        _queueOfPool.Enqueue(returnedObject);
    }
    
    private T SpawnPool()
    {
        for (int i = 0; i < _countOfSpawningPool; i++)
        {
            T spawnedObject = GameObject.Instantiate<T>(_prefab);
            if(_parent)
                spawnedObject.transform.SetParent(_parent);
            spawnedObject.gameObject.SetActive(false);
            _queueOfPool.Enqueue(spawnedObject);
            if (i == _countOfSpawningPool - 1)
            {
                spawnedObject.gameObject.SetActive(true);
                return spawnedObject;
            }
        }
        return null;
    }
}