using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberBuilder : MonoBehaviour
{
    [SerializeField] private UIDamageNumber _uiDamageNumberPrefab;
    private int maxInPool = 100;
    [SerializeField] private ObjectPool<UIDamageNumber> _pool;
    [SerializeField] private GameObject _parent;

    private void Awake()
    {
        _parent = new GameObject("UIDamageNumberParent");
        _uiDamageNumberPrefab = GameManager.current.gameInfo.uiDamageNumberPrefab.GetComponent<UIDamageNumber>();
        BuildPool(_uiDamageNumberPrefab, maxInPool, _parent.transform);
    }

    public void BuildPool(UIDamageNumber p, int bStock, Transform parent)
    {
        if (bStock > maxInPool) bStock = maxInPool;
        _uiDamageNumberPrefab = p;
        _pool = new ObjectPool<UIDamageNumber>(UIDamageNumberFactory, UIDamageNumber.TurnOn, UIDamageNumber.TurnOff, bStock, true, parent);
    }

    public UIDamageNumber UIDamageNumberFactory(Transform parent = null)
    {
        if (parent != null) return Instantiate(_uiDamageNumberPrefab, parent);
        return Instantiate(_uiDamageNumberPrefab);
    }

    public UIDamageNumber GetObject()
    {
        UIDamageNumber aux = _pool.GetObject();
        return aux;
    }

    public void ReturnObject(UIDamageNumber p)
    {
        _pool.ReturnObject(p);
    }
}
