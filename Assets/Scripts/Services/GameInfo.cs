using UnityEngine;

[CreateAssetMenu]
public class GameInfo : ScriptableObject
{
    //---VARIABLES---
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


    //---STATBLOCKS---
    public PawnStatBlock defaultStatBlock;
    public PawnStatBlock currentPlayerStatBlock;
















}
