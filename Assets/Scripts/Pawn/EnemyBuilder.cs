using UnityEngine;

public class EnemyBuilder : MonoBehaviour
{
    [SerializeField] private ObjectPool<Pawn> _pool;
    [SerializeField] private GameObject _parent;

    public void BuildPool(ObjectPool<Pawn>.FactoryMethod Factory, int bStock, Transform parent)
    {
        _pool = new ObjectPool<Pawn>(Factory, Pawn.TurnOn, Pawn.TurnOff, bStock, true, parent);
    }

    public Pawn GetObject()
    {
        Pawn aux = _pool.GetObject();
        return aux;
    }

    public void ReturnObject(Pawn p)
    {
        _pool.ReturnObject(p);
    }
}
