using UnityEngine;

[CreateAssetMenu]
public class GameInfo : ScriptableObject
{
    //---VARIABLES---
    public bool useCurrentPlayerStatBlock;
    public Vector3Variable playerPositionVar;
    public Vector3Variable alternativePositionVar;


    //---PREFABS---
    public GameObject playablePawnPrefab;
    public GameObject playerCameraPrefab;
    public GameObject enemyPawnPrefab;
    public GameObject projectilePrefab;
    public GameObject playerUIPrefab;
    public GameObject uiDamageNumberPrefab;
    public GameObject poXpPrefab;
    public GameObject poMaterialPrefab;
    public GameObject pawnUpgradePrefab;
    public GameObject weaponUpgradePrefab;
    public GameObject abilityUpgradePrefab;
    public GameObject weaponHitPrefab;


    //---STATBLOCKS---
    public PawnStatBlock defaultStatBlock;
    public PawnStatBlock currentPlayerStatBlock;

    public PawnStatBlock defaultEnemyStatBlock;

    public PawnStatBlock slimeEnemyStatBlock;
    public PawnStatBlock zombieEnemyStatBlock;
    public PawnStatBlock bugEnemyStatBlock;
















}
