using UnityEngine;

[CreateAssetMenu]
public class GameInfo : ScriptableObject
{
    //---VARIABLES---
    public bool useCurrentPlayerStatBlock;
    public Vector3Variable playerPositionVar;
    public Vector3Variable alternativePositionVar;


    //---PREFABS---
    public GameObject devConsolePrefab;
    public GameObject playablePawnPrefab;
    public GameObject playerCameraPrefab;
    public GameObject enemyPawnPrefab;

    public GameObject crosshairPrefab;

    public GameObject corpsePrefab;
    
    public GameObject projectilePrefab;
    public GameObject bounceProjectilePrefab;
    public GameObject explosiveProjectilePrefab;

    public GameObject playerUIPrefab;
    public GameObject uiDamageNumberPrefab;
    public GameObject poXpPrefab;
    public GameObject poMaterialPrefab;
    public GameObject pawnUpgradePrefab;
    public GameObject weaponUpgradePrefab;
    public GameObject abilityUpgradePrefab;
    public GameObject weaponHitPrefab;
    public GameObject explosionPrefab;
    public GameObject throwablePrefab;
    public GameObject audioSourcePrefab;


    //---STATBLOCKS---
    public PawnStatBlock defaultStatBlock;
    public PawnStatBlock currentPlayerStatBlock;

    public PawnStatBlock defaultEnemyStatBlock;

    public PawnStatBlock slimeEnemyStatBlock;
    public PawnStatBlock zombieEnemyStatBlock;
    public PawnStatBlock bugEnemyStatBlock;

    //---AUDIO---
    public AudioClip acButtonPress;

    public AudioClip acLevelUp;
    public AudioClip acXpPickUp;

    public AudioClip acPlayerSearch;
    public AudioClip acPlayerShoot;

    public AudioClip acPlayerHurt;
    public AudioClip acEnemyHurt;

    public AudioClip acExplosion;


    //---SHADERS---
    public Material defaultMaterial;
    public Material selectableMaterial;








}
