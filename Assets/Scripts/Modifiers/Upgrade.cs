using Reclaimers.DeveloperConsole;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Modifier/Upgrade")]
public class Upgrade : Modifier
{
    public UpgradeCategory uCat = UpgradeCategory.Pawn;

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

    public override string GetDesc()
    {
        for (int i = 0; i < statVarsToModify.Count; i++)
        {
            if (statVarsToModify[i].valueB > 0) modDesc = modDesc.Replace($"&valB{i}", (statVarsToModify[i].valueB * (int)quality).ToString("0.##"));
            if (statVarsToModify[i].valueM > 0) modDesc = modDesc.Replace($"&valM{i}", (statVarsToModify[i].valueM * (int)quality * 100).ToString("0.##"));
        }
        return modDesc;
    }
}
