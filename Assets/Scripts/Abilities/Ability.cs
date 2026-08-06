using UnityEngine;
using Reclaimers.UpdateSystem;
using Reclaimers.Systems;

public abstract class Ability : MonoBehaviour, IPauseable
{
    [SerializeField] protected Pawn _p;

    int curCharges = 0;

    Timer timerCooldown;

    public virtual void SetOwner(Pawn p)
    {
        _p = p;
        curCharges = _p.statBlock.abilityCharges.ValueInt();
        this.Register();

        GameManager.Instance.eventSystem.UpdateAbilityName(_p.statBlock.abilityName);
        GameManager.Instance.eventSystem.UpdateAbility(curCharges, _p.statBlock.abilityCharges.ValueInt());
    }

    public virtual void ManagedPause(bool paused)
    {
        timerCooldown?.Pause(paused);
    }

    protected virtual void OnDisable()
    {
        this.Unregister();
    }

    public virtual void AbilityUse()
    {
        if (curCharges < 1) return;
        Effect();
        curCharges--;
        GameManager.Instance.eventSystem.UpdateAbility(curCharges, _p.statBlock.abilityCharges.ValueInt());
        GameManager.Instance.eventSystem.UpdateAbilityReload(0f, 1f, true);

        if (!timerCooldown)
        {
            timerCooldown = GameManager.Instance.timerSystem.SetTimer(_p.statBlock.abilityCooldown.Value(), CooldownEnd, 0.01f, () =>
            {
                GameManager.Instance.eventSystem.UpdateAbilityReload(timerCooldown.durationElapsed, _p.statBlock.abilityCooldown.Value(), true);
            }, "ABILITY_Cooldown");
        }
    }

    public virtual void CooldownPartial()
    {

    }

    public virtual void CooldownEnd()
    {
        curCharges++;
        GameManager.Instance.eventSystem.UpdateAbility(curCharges, _p.statBlock.abilityCharges.ValueInt());
        timerCooldown = null;
        if (curCharges < _p.statBlock.abilityCharges.ValueInt())
        {
            timerCooldown = GameManager.Instance.timerSystem.SetTimer(_p.statBlock.abilityCooldown.Value(), CooldownEnd, 0.01f, () =>
            {
                GameManager.Instance.eventSystem.UpdateAbilityReload(timerCooldown.durationElapsed, _p.statBlock.abilityCooldown.Value(), true);
            }, "ABILITY_Cooldown");
        } else
        {
            GameManager.Instance.eventSystem.UpdateAbilityReload(0f, 1f, false);
        }
    }

    protected virtual void Effect()
    {

    }

}
