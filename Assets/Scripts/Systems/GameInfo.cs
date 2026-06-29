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

    public WeaponBlock defaultWeaponBlock;


    public GameObject pawnPrefab;
    public GameObject projectilePrefab;
    public GameObject floatingTextPrefab;
    public GameObject xpOrbPrefab;
    public GameObject moneyOrbPrefab;
    public GameObject obstaclePartPrefab;
}
