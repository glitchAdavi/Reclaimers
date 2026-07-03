using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Modifier/Upgrade")]
public class Upgrade : Modifier
{
    public override void Apply(Pawn target)
    {
        base.Apply(target);
        ModifyStats();
    }

    public override void Remove(Pawn target)
    {
        base.Remove(target);
        ModifyStats(true);
    }


}
