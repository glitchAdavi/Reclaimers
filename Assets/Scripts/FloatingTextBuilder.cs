using System.Collections.Generic;
using UnityEngine;

public class FloatingTextBuilder : MonoBehaviour
{
    [SerializeField] private ObjectPool<FloatingText> _pool;
    [SerializeField] private GameObject _parent;

    public void BuildPool(ObjectPool<FloatingText>.FactoryMethod Factory, int bStock, Transform parent)
    {
        _pool = new ObjectPool<FloatingText>(Factory, FloatingText.TurnOn, FloatingText.TurnOff, bStock, true, parent);
    }

    public FloatingText GetObject()
    {
        FloatingText aux = _pool.GetObject();
        return aux;
    }

    public void ReturnObject(FloatingText ft)
    {
        _pool.ReturnObject(ft);
    }

    public void DestroyBuilder()
    {
        List<FloatingText> toDestroy = _pool.EmptyPool();

        for (int i = toDestroy.Count - 1; i >= 0; i--)
        {
            FloatingText aux = toDestroy[i];
            toDestroy.RemoveAt(i);
            Destroy(aux);
        }

        Destroy(this);
    }
}
