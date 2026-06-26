using UnityEngine;
using Reclaimers.UpdateSystem;
using Reclaimers.Systems;

public enum AbilityType
{
    TEST
}

public abstract class Ability : MonoBehaviour, IPauseable
{
    [SerializeField] private Pawn owner;

    int curCharges = 0;

    Timer timerCooldown;

    protected virtual void OnEnable()
    {
        curCharges = owner.abilityBlock.abilityCharges.ValueInt();

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
            timerCooldown = GameManager.Instance.timerSystem.SetTimer(owner.abilityBlock.abilityCooldown.Value(), CooldownEnd);
        }
    }

    public virtual void CooldownPartial()
    {

    }

    public virtual void CooldownEnd()
    {
        curCharges++;
        timerCooldown = null;
        if (curCharges < owner.abilityBlock.abilityCharges.ValueInt())
        {
            timerCooldown = GameManager.Instance.timerSystem.SetTimer(owner.abilityBlock.abilityCooldown.Value(), CooldownEnd);
        }
    }

    protected virtual void Effect()
    {

    }
}
