using UnityEngine;
using Reclaimers.UpdateSystem;
using Reclaimers.Systems;

public enum AbilityType
{
    TEST
}

public abstract class Ability : MonoBehaviour, IPauseable
{
    [SerializeField] private Pawn p;

    int curCharges = 0;

    Timer timerCooldown;

    protected virtual void OnEnable()
    {
        curCharges = p.statBlock.abilityCharges.ValueInt();

        this.Register();
    }

    public virtual void ManagedPause(bool paused)
    {
        timerCooldown?.Pause(paused);
    }

    protected virtual void OnDisable()
    {
        this.Unregister();
    }

    protected virtual void AbilityUse()
    {
        if (curCharges < 1) return;
        Effect();
        curCharges--;

        if (!timerCooldown)
        {
            timerCooldown = GameManager.Instance.timerSystem.SetTimer(p.statBlock.abilityCooldown.Value(), CooldownEnd);
        }
    }

    public virtual void CooldownPartial()
    {

    }

    public virtual void CooldownEnd()
    {
        curCharges++;
        timerCooldown = null;
        if (curCharges < p.statBlock.abilityCharges.ValueInt())
        {
            timerCooldown = GameManager.Instance.timerSystem.SetTimer(p.statBlock.abilityCooldown.Value(), CooldownEnd);
        }
    }

    protected virtual void Effect()
    {

    }
}
