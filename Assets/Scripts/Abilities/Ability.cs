using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour, IPause
{
    public PlayablePawn owner;

    public AbilityStatBlock baseStatBlock;
    public AbilityStatBlock statBlock;

    public float aCooldown;
    public int aMaxCharges;
    public int aCharges;
    public float aDamage;

    Timer timerCooldown;

    protected virtual void OnEnable()
    {
        GameManager.current.updateService.RegisterPause(this);
    }

    public void Pause(bool paused)
    {
        timerCooldown?.Pause(paused);
    }

    protected virtual void OnDisable()
    {
        GameManager.current.updateService.UnregisterPause(this);
    }

    public void UseAbility()
    {
        if (aCharges < 1) return;
        Effect();
        aCharges--;
        if (aMaxCharges > 1) GameManager.current.eventService.RequestUIUpdateAbilityCharges(aCharges);
        if (timerCooldown == null)
        {
            GameManager.current.eventService.RequestUIUpdateAbilitySlider(0f, aCooldown);
            timerCooldown = GameManager.current.timerService.StartTimer(aCooldown, CooldownEffect, Time.fixedDeltaTime, CooldownPartial);
        }
    }

    public void CooldownPartial()
    {
        GameManager.current.eventService.RequestUIUpdateAbilitySlider(timerCooldown.lifeTime, aCooldown);
    }

    public void CooldownEffect()
    {
        aCharges++;
        if (aMaxCharges > 1) GameManager.current.eventService.RequestUIUpdateAbilityCharges(aCharges);
        timerCooldown = null;
        if (aCharges < aMaxCharges)
        {
            GameManager.current.eventService.RequestUIUpdateAbilitySlider(0f, aCooldown);
            timerCooldown = GameManager.current.timerService.StartTimer(aCooldown, CooldownEffect, Time.fixedDeltaTime, CooldownPartial);
        }
    }

    public virtual void Effect()
    { 
        
    }


    #region ApplyValues
    public virtual void FirstStatApplication()
    {
        statBlock = ScriptableObject.CreateInstance<AbilityStatBlock>();
        statBlock.CopyValues(baseStatBlock);

        ApplyCooldown();
        ApplyCharges();

        aCharges = aMaxCharges;

        GameManager.current.eventService.RequestUIAbilityShow(true);
        GameManager.current.eventService.RequestUIUpdateAbilityName(statBlock.abilityName);
        if (aMaxCharges > 1) GameManager.current.eventService.RequestUIUpdateAbilityCharges(aCharges);
        GameManager.current.eventService.RequestUIUpdateAbilitySlider(aCooldown, aCooldown);
    }

    public void ApplyCooldown()
    {
        aCooldown = statBlock.abilityCooldown.Value();
    }

    public void ApplyCharges()
    {
        aMaxCharges = statBlock.abilityCharges.ValueInt();
    }

    public void ApplyDamage()
    {
        aDamage = statBlock.abilityDamage.Value();
    }


    #endregion
}
