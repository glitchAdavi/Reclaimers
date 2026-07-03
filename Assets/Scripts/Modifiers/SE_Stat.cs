using UnityEngine;

[CreateAssetMenu(fileName = "SE_Stat", menuName = "Modifier/StatusEffect/SE_Stat")]
public class SE_Stat : StatusEffect
{
    public override void OnStatusEffectStart()
    {
        ModifyStats();
    }

    public override void OnStatusEffectEnd()
    {
        ModifyStats(true);

        base.OnStatusEffectEnd();
    }
}
