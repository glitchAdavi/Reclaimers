using UnityEngine;

[CreateAssetMenu(fileName = "SE_DoT", menuName = "StatusEffect/SE_DoT")]
public class SE_DoT : StatusEffect
{
    public override void OnStatusEffectTick()
    {
        target.GetHit(value, false);
    }
}
