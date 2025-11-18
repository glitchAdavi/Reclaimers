using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextBuilder : MonoBehaviour
{
    [SerializeField] private UIFloatingText _uiFloatingTextPrefab;
    private int maxInPool = 100;
    [SerializeField] private ObjectPool<UIFloatingText> _pool;
    [SerializeField] private GameObject _parent;

    private void Awake()
    {
        _parent = new GameObject("UIDamageNumberParent");
        _uiFloatingTextPrefab = GameManager.current.gameInfo.uiDamageNumberPrefab.GetComponent<UIFloatingText>();
        BuildPool(_uiFloatingTextPrefab, maxInPool, _parent.transform);
    }

    public void BuildPool(UIFloatingText p, int bStock, Transform parent)
    {
        if (bStock > maxInPool) bStock = maxInPool;
        _uiFloatingTextPrefab = p;
        _pool = new ObjectPool<UIFloatingText>(UIFloatingTextFactory, UIFloatingText.TurnOn, UIFloatingText.TurnOff, bStock, true, parent);
    }

    public UIFloatingText UIFloatingTextFactory(Transform parent = null)
    {
        if (parent != null) return Instantiate(_uiFloatingTextPrefab, parent);
        return Instantiate(_uiFloatingTextPrefab);
    }

    public UIFloatingText GetObject()
    {
        UIFloatingText aux = _pool.GetObject();
        return aux;
    }

    public void ReturnObject(UIFloatingText p)
    {
        _pool.ReturnObject(p);
    }
}
