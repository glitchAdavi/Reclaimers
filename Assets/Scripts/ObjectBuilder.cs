using System.Collections.Generic;
using UnityEngine;

public class ObjectBuilder<T>
{
    [SerializeField] private ObjectPool<T> _pool;
    [SerializeField] private GameObject _parent;

    public void BuildPool(ObjectPool<T>.FactoryMethod Factory, int bStock, Transform parent)
    {
        _pool = new ObjectPool<T>(Factory, bStock, true, parent);
    }

    public T GetObject()
    {
        T aux = _pool.GetObject();
        return aux;
    }

    public void ReturnObject(T t)
    {
        _pool.ReturnObject(t);
    }

    public void DestroyBuilder()
    {
        List<T> toDestroy = _pool.EmptyPool();

        for (int i = toDestroy.Count - 1; i >= 0; i--)
        {
            T aux = toDestroy[i];
            toDestroy.RemoveAt(i);
            GameObject.Destroy(aux as Object);
        }
    }
}
