using UnityEngine;

[CreateAssetMenu]
public class GameInfo : ScriptableObject
{
    //---VARIABLES---
    public Vector3Variable playerPositionVar;
    public Vector3Variable alternativePositionVar;

    public StatBlock defaultPlayerStatBlock;
    public StatBlock tempPlayerStatBlock;

    public StatBlock defaultEnemyStatBlock;
    public StatBlock slimeEnemyStatBlock;
    public StatBlock zombieEnemyStatBlock;
    public StatBlock bugEnemyStatBlock;
    public StatBlock staticPlantEnemyStatBlock;
    public StatBlock ratEnemyStatBlock;
    public StatBlock mimicEnemyStatBlock;
    public StatBlock amalgamStatBlock;
    public StatBlock amalgamPartStatBlock;

    public StatBlock slimeBossStatBlack;
    public StatBlock amalgamBossStatBlack;

    public StatBlock defaultWeaponBlock;
    public StatBlock E_Pistol;

    public StatBlock defaultAbilityBlock;

    public GameObject devConsolePrefab;
    public GameObject pawnPrefab;
    public GameObject playerCameraPrefab;
    public GameObject projectilePrefab;
    public GameObject floatingTextPrefab;
    public GameObject xpOrbPrefab;
    public GameObject moneyOrbPrefab;
    public GameObject obstaclePartPrefab;
    public GameObject audioSourcePrefab;
    public GameObject playerUIPrefab;
    public GameObject hitPrefab;
    public GameObject upgradePrefab;


    public Sprite hitWall;
    public Sprite hitPlayer;
    public Sprite hitEnemy;
    public Sprite upgradePawn;
    public Sprite upgradeWeapon;
    public Sprite upgradeAbility;

    public Material spriteShader;
    public Material selectableShader;
    
}
