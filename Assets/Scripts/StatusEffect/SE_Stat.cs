using UnityEngine;

[CreateAssetMenu(fileName = "SE_DoT", menuName = "StatusEffect/SE_Stat")]
public class SE_Stat : StatusEffect
{
    public override void OnStatusEffectStart()
    {
        ModifyAllStats();
    }

    public override void OnStatusEffectEnd()
    {
        ModifyAllStats(true);

        base.OnStatusEffectEnd();
    }
}
