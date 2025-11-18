using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PawnService : MonoBehaviour, IUpdate, IPause
{
    public EnemyBuilder enemyBuilder;

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private List<Pawn> pawnsInScene = new List<Pawn>();
    [SerializeField] private List<PlayablePawn> playablePawnsInScene = new List<PlayablePawn>();

    //temp
    public Vector3 fixedSpawnPoint = new Vector3(10f, 0f, 10f);

    public float spawnTimer = 0f;
    public float spawnTimerLimit = 0.1f;

    public int maxSpawnedEnemies = 10;
    public int spawnedEnemies = 0;

    public bool pawnServiceActive = false;
    public bool canEnemiesSpawn = false;
    public bool isPaused = false;


    public void OnEnable()
    {

        enemyBuilder = GameManager.current.CreateService<EnemyBuilder>();
        _enemyPrefab = GameManager.current.gameInfo.enemyPawnPrefab;

        GameManager.current.eventService.onEnemyDeath += OnEnemyDeath;
        GameManager.current.eventService.onPawnServiceActive += SetPawnServiceActive;

        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);
    }


    public void ExecuteUpdate()
    {
        if (!pawnServiceActive) return;

        if (canEnemiesSpawn && spawnedEnemies < maxSpawnedEnemies)
        {
            SpawnEnemy();
            canEnemiesSpawn = false;
            spawnTimer = 0;
        }

        if (spawnTimerLimit > spawnTimer)
        {
            spawnTimer += Time.deltaTime;
        } else
        {
            spawnTimer = 0;
            canEnemiesSpawn = true;
        }

    }

    public void Pause(bool paused)
    {
        isPaused = paused;
        canEnemiesSpawn = !paused;
    }

    public void OnDisable()
    {
        GameManager.current.updateService.UnregisterUpdate(this);
        GameManager.current.updateService.UnregisterPause(this);
    }


    #region Spawn
    public void SetPawnServiceActive(bool active)
    {
        if (isPaused) return;
        pawnServiceActive = active;
    }


    //TEMP SPAWN

    public void SpawnEnemy()
    {
        EnemyPawn newEnemy = enemyBuilder.GetObject();
        newEnemy.Teleport(fixedSpawnPoint);
        pawnsInScene.Add(newEnemy);
        spawnedEnemies++;
    }

    // END TEMP SPAWN


    #endregion

    public Pawn GetTarget()
    {
        return GameManager.current.playerPawn;
    }

    public void OnEnemyDeath(EnemyPawn e)
    {
        pawnsInScene.Remove(e);
        spawnedEnemies--;
    }

    public PlayablePawn GetClosestPlayablePawn(Vector3 pos, float range)
    {
        if (playablePawnsInScene.Count <= 0) return null;

        PlayablePawn result = null;

        for (int i = 0; i < playablePawnsInScene.Count; i++)
        {
            if (Vector3.Distance(playablePawnsInScene[i].transform.position, pos) <= range) result = playablePawnsInScene[i];
        }

        return result;
    }
}
