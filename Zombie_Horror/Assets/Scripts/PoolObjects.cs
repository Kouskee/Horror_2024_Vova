using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PoolObjects
{
    private Transform _parent;
    private Object _prefab;
    private Queue<Object> _poolObjects;

    public PoolObjects(Object prefab, int startPoolCount, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;
        _poolObjects = new Queue<Object>(startPoolCount);
        for (int i = 0; i < startPoolCount; i++)
        {
            var newObject = Object.Instantiate(prefab, _parent);
            _poolObjects.Enqueue(newObject);
        }
    }

    public Object SpawnObject()
    {
        if (_poolObjects.Count <= 0) return Object.Instantiate(_prefab, _parent);

        var poolObject = _poolObjects.Dequeue();
        return poolObject;
    }

    public void ReturnToPool(Object chunk)
    {
        _poolObjects.Enqueue(chunk);
    }
}