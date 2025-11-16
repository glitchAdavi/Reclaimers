using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    public delegate T FactoryMethod(Transform parent = null);
    private FactoryMethod _factoryMethod;

    [SerializeField] public List<T> _currentStock;
    private bool _isDynamic;

    private Action<T> _turnOnCallback;
    private Action<T> _turnOffCallback;

    private Transform _parent;

    public ObjectPool(FactoryMethod factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback, int initialStock = 0, bool isDynamic = true, Transform parent = null)
    {
        _factoryMethod = factoryMethod;
        _turnOnCallback = turnOnCallback;
        _turnOffCallback = turnOffCallback;

        _isDynamic = isDynamic;

        _parent = parent;

        _currentStock = new List<T>();

        for (int i = 0; i < initialStock; i++)
        {
            var obj = _factoryMethod(_parent);
            _turnOffCallback(obj);
            _currentStock.Add(obj);
        }
    }

    public T GetObject()
    {
        var result = default(T);

        if (_currentStock.Count > 0)
        {
            result = _currentStock[0];
            _currentStock.RemoveAt(0);
        }
        else if (_isDynamic)
        {
            result = _factoryMethod(_parent);
        }

        _turnOnCallback(result);
        return result;
    }

    public void ReturnObject(T obj)
    {
        _turnOffCallback(obj);
        _currentStock.Add(obj);
    }

    public List<T> EmptyPool()
    {
        List<T> result = new List<T>(_currentStock);
        _currentStock.Clear();
        return result;
    }

    public int Stock()
    {
        return _currentStock.Count;
    }
}
