using System.Collections.Generic;
using UnityEngine;

public class ProjectileBuilder : MonoBehaviour
{
    [SerializeField] private ObjectPool<Projectile> _pool;
    [SerializeField] private GameObject _parent;

    public void BuildPool(ObjectPool<Projectile>.FactoryMethod Factory, int bStock, Transform parent)
    {
        _pool = new ObjectPool<Projectile>(Factory, Projectile.TurnOn, Projectile.TurnOff, bStock, true, parent);
    }

    public Projectile GetObject()
    {
        Projectile aux = _pool.GetObject();
        return aux;
    }

    public void ReturnObject(Projectile p)
    {
        _pool.ReturnObject(p);
    }

    public void DestroyBuilder()
    {
        List<Projectile> toDestroy = _pool.EmptyPool();

        for (int i = toDestroy.Count - 1; i >= 0; i--)
        {
            Projectile aux = toDestroy[i];
            toDestroy.RemoveAt(i);
            Destroy(aux);
        }

        Destroy(this);
    }
}
