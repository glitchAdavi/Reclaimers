using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuilder : MonoBehaviour
{
    [SerializeField] private EnemyPawn _enemyPrefab;
    private int maxInPool = 100;
    [SerializeField] private ObjectPool<EnemyPawn> _pool;
    [SerializeField] private GameObject _parent;

    private void Awake()
    {
        _parent = new GameObject("EnemyParent");
        _enemyPrefab = GameManager.current.gameInfo.enemyPawnPrefab.GetComponent<EnemyPawn>();
        BuildPool(_enemyPrefab, maxInPool, _parent.transform);
    }

    public void BuildPool(EnemyPawn p, int bStock, Transform parent)
    {
        if (bStock > maxInPool) bStock = maxInPool;
        _enemyPrefab = p;
        _pool = new ObjectPool<EnemyPawn>(EnemyFactory, EnemyPawn.TurnOn, EnemyPawn.TurnOff, bStock, true, parent);
    }

    public EnemyPawn EnemyFactory(Transform parent = null)
    {
        if (parent != null) return Instantiate(_enemyPrefab, parent);
        return Instantiate(_enemyPrefab);
    }

    public EnemyPawn GetObject()
    {
        EnemyPawn aux = _pool.GetObject();
        return aux;
    }

    public void ReturnEnemyPawn(EnemyPawn p)
    {
        _pool.ReturnObject(p);
    }
}
