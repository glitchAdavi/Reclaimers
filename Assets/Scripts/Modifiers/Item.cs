using UnityEngine;


public abstract class Item : Modifier
{
    public override void Apply(Pawn target)
    {
        target.activeItems.Add(this);
    }

    public virtual void UpdateItem(Pawn p, int stacks)
    {

    }
}

public class DamageAura : Item
{
    public override void UpdateItem(Pawn p, int stacks)
    {
        foreach (Pawn e in p.p_nearbyEnemies)
        {
            e.GetHit(p.statBlock.projDamage.Value() * stacks);
        }
    }
}

public class SlowAura : Item
{
    public override void UpdateItem(Pawn p, int stacks)
    {
        if (stacks < 1) return;
        foreach (Pawn e in p.p_nearbyEnemies)
        {
            e.AddStatusEffect(GameManager.Instance.GetModifier("se_stat_slow") as StatusEffect);
        }
    }
}
