using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayablePawn : Pawn
{
    public Weapon equippedWeapon;


    //TEMP
    public int levelThreshold = 5;

    //TEMP


    protected override void OnEnable()
    {
        base.OnEnable();

        FirstStatApplication();

        ApplyWeapon();

        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);

        GameManager.current.eventService.onGivePlayerXp += GainXp;
        GameManager.current.eventService.onGivePlayerLevel += GainLevel;


        //GameManager.current.timerService.StartTimer(5f, () => Debug.Log("FINAL"), 1f, () => Debug.Log("PARTIAL"));
    }

    protected override void PawnUpdate()
    {
        base.PawnUpdate();
    }

    protected override void PawnPause()
    {
        base.PawnPause();

        // necessary or else pawn drifts for a moment if moving while pausing
        _nav.velocity = Vector3.zero;

        if (isPaused) _nav.isStopped = true;
        else _nav.isStopped = false;
    }

    protected override void OnDisable()
    {
        GameManager.current.updateService.UnregisterUpdate(this);
        GameManager.current.updateService.UnregisterPause(this);
    }

    protected override void FirstStatApplication()
    {
        base.FirstStatApplication();
        ApplyLpRegen();
        ApplyLpRegenDelay();
        ApplyLpRegenTickTime();
        ApplyHealingMultiplier();
        ApplyIFrameDuration();
        ApplyXp();
        ApplyXpGain();
        ApplyLevel();
        ApplyPickUpRange();

        GameManager.current.eventService.RequestUIUpdateHealth(lifepoints, maxLifepoints);
        GameManager.current.eventService.RequestUIUpdateXpBar(xp, levelThreshold);
        GameManager.current.eventService.RequestUIUpdateLevelCounter(level);
    }


    public void Move(float xInput, float yInput)
    {
        yInput *= 1.4f;

        Vector3 movement = new Vector3(xInput, 0f, yInput);
        Vector3 moveDest = transform.position + (movement / 2);

        _nav.destination = moveDest;

        GameManager.current.gameInfo.playerPositionVar.SetValue(transform.position);
    }

    public override void GetHit(float damage, bool isCrit, float knockback = 0, Vector3? knockbackPush = null)
    {
        base.GetHit(damage, isCrit, knockback, knockbackPush);

        GameManager.current.eventService.RequestUIUpdateHealth(lifepoints, maxLifepoints);
    }

    public override void Heal(float value)
    {
        base.Heal(value);

        GameManager.current.eventService.RequestUIUpdateHealth(lifepoints, maxLifepoints);
    }

    #region XP
    public void GainXp(float v)
    {
        totalXp += v;
        xp += v;
        while (xp >= levelThreshold)
        {
            xp -= levelThreshold;
            GainLevel(1);
        }

        GameManager.current.eventService.RequestUIUpdateXpBar(xp, levelThreshold);
    }

    public void GainLevel(int l)
    {
        level++;
        GameManager.current.eventService.RequestUIUpdateLevelCounter(level);
    }
    #endregion

    public WeaponStatBlock ChangeWeapon(WeaponStatBlock newWeapon)
    {
        WeaponStatBlock currentWeapon = statBlock.equippedWeapon;
        statBlock.equippedWeapon = newWeapon;
        ApplyWeapon();
        return currentWeapon;
    }

    private void ApplyWeapon()
    {
        if (statBlock.equippedWeapon != null)
        {
            Weapon currentWeapon = equippedWeapon;

            Type wType = Type.GetType(statBlock.equippedWeapon.weaponType);
            equippedWeapon = gameObject.AddComponent(wType) as Weapon;
            equippedWeapon.statBlock = statBlock.equippedWeapon;
            equippedWeapon.FirstStatApplication();

            if (equippedWeapon != null) Destroy(currentWeapon);
        }
    }
}
