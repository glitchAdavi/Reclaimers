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
    public StatBlock movingPlantEnemyStatBlock;
    public StatBlock wolfEnemyStatBlock;
    public StatBlock fleshGolemStatBlock;

    public StatBlock defaultWeaponBlock;

    public StatBlock defaultAbilityBlock;


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


    public Sprite hitWall;
    public Sprite hitPlayer;
    public Sprite hitEnemy;
    
}
