using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBuilder : MonoBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;
    private int maxInPool = 100;
    [SerializeField] private ObjectPool<Projectile> _pool;
    [SerializeField] private GameObject _parent;

    private void Awake()
    {
        _parent = new GameObject("ProjectileParent");
        _projectilePrefab = GameManager.current.gameInfo.projectilePrefab.GetComponent<Projectile>();
        BuildPool(_projectilePrefab, maxInPool, _parent.transform);
    }

    public void NewPool(Projectile p, int bStock)
    {
        BuildPool(p, maxInPool, _parent.transform);
    }

    public void BuildPool(Projectile p, int bStock, Transform parent)
    {
        if (_pool != null)
        {
            for (int i = _parent.transform.childCount - 1; i >= 0; i--)
            {
                GameObject child = _parent.transform.GetChild(i).gameObject;
                Destroy(child);
            }
            _pool = null;
        }

        if (bStock > maxInPool) bStock = maxInPool;
        _projectilePrefab = p;
        _pool = new ObjectPool<Projectile>(ProjectileFactory, Projectile.TurnOn, Projectile.TurnOff, bStock, true, parent);
    }

    public Projectile ProjectileFactory(Transform parent = null)
    {
        if (parent != null) return Instantiate(_projectilePrefab, parent);
        return Instantiate(_projectilePrefab);
    }

    public Projectile GetObject()
    {
        Projectile aux = _pool.GetObject();
        return aux;
    }

    public void ReturnProjectile(Projectile p)
    {
        _pool.ReturnObject(p);
    }
}
