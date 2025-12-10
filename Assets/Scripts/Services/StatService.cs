using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatService : MonoBehaviour
{
    [SerializeField] protected int enemiesKilled = 0;
    [SerializeField] protected float xpGained = 0;
    [SerializeField] protected int levelsGained = 0;
    [SerializeField] protected float materialsGained = 0;
    [SerializeField] protected int pawnUpgradesGained = 0;
    [SerializeField] protected int weaponUpgradesGained = 0;
    [SerializeField] protected int abilityUpgradesGained = 0;

    private void OnEnable()
    {
        GameManager.current.eventService.onEnemyDeath += AddEnemyKilled;
        GameManager.current.eventService.onGivePlayerXp += AddXpGained;
        GameManager.current.eventService.onGivePlayerLevel += AddLevelGained;
        GameManager.current.eventService.onGivePlayerMaterial += AddMaterialsGained;
    }

    public void AddEnemyKilled(EnemyPawn e)
    {
        enemiesKilled++;
        GameManager.current.eventService.RequestUIUpdateKillCount(enemiesKilled);
    }

    public void AddXpGained(float gained)
    {
        xpGained += gained;
        GameManager.current.eventService.RequestUIUpdateXpGained(xpGained);
    }

    public void AddLevelGained(int gained, bool t = false)
    {
        levelsGained += gained;
    }

    public void AddMaterialsGained(float gained)
    {
        materialsGained += gained;
        GameManager.current.eventService.RequestUIUpdateMaterialsGained(materialsGained);
    }

    public void AddPawnUpgradeGained()
    {
        pawnUpgradesGained++;
    }

    public void AddWeaponUpgradeGained()
    {
        weaponUpgradesGained++;
    }

    public void AddAbilityUpgradeGained()
    {
        abilityUpgradesGained++;
    }
}
