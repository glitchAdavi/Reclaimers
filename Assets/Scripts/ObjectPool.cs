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

    private Transform _parent;

    public ObjectPool(FactoryMethod factoryMethod, int initialStock = 0, bool isDynamic = true, Transform parent = null)
    {
        _factoryMethod = factoryMethod;

        _isDynamic = isDynamic;

        _parent = parent;

        _currentStock = new List<T>();

        for (int i = 0; i < initialStock; i++)
        {
            T obj = _factoryMethod(_parent);
            (obj as MonoBehaviour).gameObject.SetActive(false);
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

        (result as MonoBehaviour).gameObject.SetActive(true);
        return result;
    }

    public void ReturnObject(T obj)
    {
        (obj as MonoBehaviour).gameObject.SetActive(false);
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
