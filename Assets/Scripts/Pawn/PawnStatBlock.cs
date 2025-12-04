using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PawnStatBlock", menuName = "Stat Block/PawnStatBlock")]
public class PawnStatBlock : ScriptableObject
{
    public Rarity rarity = Rarity.Common;

    public string pawnName = "John Doe";
    public string pawnDescription = "A pawn.";
    public WeaponStatBlock equippedWeapon;
    public AbilityStatBlock equippedAbility;

    public RuntimeAnimatorController controller;

    public List<string> keyUpgrade;
    public List<int> valueUpgrade;

    public Sprite pawnSprite;
    public Color pawnSpriteColor = new Color(255, 255, 255, 255);

    public Stat scale = new Stat(1f, 0f, 1f);

    public Stat materials = new Stat(0f, 0f, 1f);
    public Stat materialGain = new Stat(1f, 0f, 1f);

    public Stat totalXp = new Stat(0f, 0f, 1f);
    public Stat xp = new Stat(0f, 0f, 1f);
    public Stat xpGain = new Stat(1f, 0f, 1f);
    public Stat level = new Stat(0f, 0f, 1f);



    public Stat speed = new Stat(1f, 0f, 1f);


    public Stat lifepoints = new Stat(100f, 0f, 1f);

    public Stat lpRegen = new Stat(0f, 0f, 1f);
    public Stat lpRegenDelay = new Stat(1f, 0f, 1f);
    public Stat lpRegenTickTime = new Stat(1f, 0f, 1f);

    public Stat damageMultiplier = new Stat(1f, 0f, 1f);
    public Stat healingMultiplier = new Stat(1f, 0f, 1f);


    public Stat meleeDamage = new Stat(1f, 0f, 1f);
    public Stat meleeCooldown = new Stat(0.5f, 0f, 1f);


    public Stat pickUpRange = new Stat(5f, 0f, 1f);
    public Stat interactionRange = new Stat(3f, 0f, 1f);

    public Stat iFrameDuration = new Stat(0.1f, 0f, 0f);

    public Stat knockBackResist = new Stat(0f, 0f, 1f);

    public Stat xpKillValue = new Stat(1f, 0f, 1f);


    public bool onDamageSpawnEnemy;
    public PawnStatBlock onDamageEnemyToSpawn;
    public Stat onDamageInterval = new Stat(0f, 0f, 1f);
    public Stat onDamageNumToSpawn = new Stat(0f, 0f, 1f);

    public bool onDeathSpawnEnemy;
    public PawnStatBlock onDeathEnemyToSpawn;
    public Stat onDeathNumToSpawn = new Stat(0f, 0f, 1f);

    public void CopyValues(PawnStatBlock psb)
    {
        rarity = psb.rarity;

        pawnName = psb.pawnName;
        pawnDescription = psb.pawnDescription;
        equippedWeapon = psb.equippedWeapon;
        equippedAbility = psb.equippedAbility;

        controller = psb.controller;

        keyUpgrade = new List<string>(psb.keyUpgrade);
        valueUpgrade = new List<int>(psb.valueUpgrade);

        pawnSprite = psb.pawnSprite;
        pawnSpriteColor = psb.pawnSpriteColor;

        scale.SetValues(psb.scale);

        materials.SetValues(psb.materials);
        materialGain.SetValues(psb.materialGain);

        totalXp.SetValues(psb.totalXp);
        xp.SetValues(psb.xp);
        xpGain.SetValues(psb.xpGain);
        level.SetValues(psb.level);

        speed.SetValues(psb.speed);


        lifepoints.SetValues(psb.lifepoints);

        lpRegen.SetValues(psb.lpRegen);
        lpRegenDelay.SetValues(psb.lpRegenDelay);
        lpRegenTickTime.SetValues(psb.lpRegenTickTime);

        damageMultiplier.SetValues(psb.damageMultiplier);
        healingMultiplier.SetValues(psb.healingMultiplier);


        meleeDamage.SetValues(psb.meleeDamage);
        meleeCooldown.SetValues(psb.meleeCooldown);

        pickUpRange.SetValues(psb.pickUpRange);
        interactionRange.SetValues(psb.interactionRange);

        iFrameDuration.SetValues(psb.iFrameDuration);

        knockBackResist.SetValues(psb.knockBackResist);

        xpKillValue.SetValues(psb.xpKillValue);

        onDamageSpawnEnemy = psb.onDamageSpawnEnemy;
        onDamageEnemyToSpawn = psb.onDamageEnemyToSpawn;
        onDamageInterval.SetValues(psb.onDamageInterval);
        onDamageNumToSpawn.SetValues(psb.onDamageNumToSpawn);

        onDeathSpawnEnemy = psb.onDeathSpawnEnemy;
        onDeathEnemyToSpawn = psb.onDeathEnemyToSpawn;
        onDeathNumToSpawn.SetValues(psb.onDeathNumToSpawn);
    }
}
