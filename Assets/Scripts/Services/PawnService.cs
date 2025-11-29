using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

public struct SpawnBlock
{
    public PawnStatBlock pawn;
    public int weight;

    public SpawnBlock(PawnStatBlock pawn, int weight)
    {
        this.pawn = pawn;
        this.weight = weight;
    }
}

public class PawnService : MonoBehaviour, IUpdate, IPause
{
    public EnemyBuilder enemyBuilder;

    [SerializeField] private Vector3 spawnPosCenter;

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private ExecutableList<Pawn> pawnsInScene = new ExecutableList<Pawn>();
    [SerializeField] private ExecutableList<PlayablePawn> playablePawnsInScene = new ExecutableList<PlayablePawn>();

    public List<Vector3> fixedSpawnPoints = new List<Vector3>();
    public float fixedSpawnPointRange = 10f;
    public Vector3 fixedSpawnPoint = new Vector3(10f, 0f, 10f);

    private List<Vector3Int> spawnableTiles = new List<Vector3Int>();
    public Vector3Int spawnArea = new Vector3Int(20, 14, 3);

    public float spawnTimer = 0f;
    public float spawnTimerLimit = 0.1f;
    public int spawnBatch = 1;
    public int maxSimultaneousEnemies = 30;
    public int spawnedEnemies = 0;




    public bool enemySpawnActive = false;
    public bool spawnAlert = false;
    public bool spawnIdle = false;
    public bool isPaused = false;


    public void OnEnable()
    {

        enemyBuilder = GameManager.current.CreateService<EnemyBuilder>();
        _enemyPrefab = GameManager.current.gameInfo.enemyPawnPrefab;

        GetAllPlayablePawnsInScene();
        InitializeInactivePlayablePawns();

        GameManager.current.eventService.onEnemyDeath += OnEnemyDeath;
        GameManager.current.eventService.onPawnServiceActive += SetPawnServiceActive;
        GameManager.current.eventService.onPawnServiceSpawnIdle += (x) => spawnIdle = x;
        GameManager.current.eventService.onPawnServiceSpawnAlert += (x) => spawnAlert = x;

        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);
    }


    public void ExecuteUpdate()
    {
        GetSpawnableTiles();
        TrySpawn();

    }

    public void Pause(bool paused)
    {
        isPaused = paused;
        spawnAlert = !paused;
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
        enemySpawnActive = active;
    }

    public void GetSpawnableTiles()
    {
        spawnPosCenter = new Vector3(GameManager.current.gameInfo.playerPositionVar.Value.x, GameManager.current.gameInfo.playerPositionVar.Value.z * 0.7f, 0);
        spawnableTiles = GameManager.current.tileService.GetAllTilesInRangeFromPos(spawnPosCenter, spawnArea.x, spawnArea.y, spawnArea.z);
    }

    public Vector3Int GetRandomSpawnableTile()
    {
        return spawnableTiles[Random.Range(0, spawnableTiles.Count)];
    }

    public void TrySpawn()
    {
        if (!enemySpawnActive) return;

        if (spawnIdle)
        {
            //SpawnEnemyIdle();

            for (int i = 0; i < fixedSpawnPoints.Count; i++)
            {
                SpawnEnemyIdle();
            }

        } 

        if (spawnAlert)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnTimerLimit)
            {
                spawnTimer = 0f;
                for (int i = 0; i < spawnBatch; i++)
                {
                    SpawnEnemyActive();
                    if (spawnedEnemies >= maxSimultaneousEnemies) break;
                }
            }
        }
    }

    public void SpawnEnemyIdle()
    {
        Debug.Log("Spawn Idle");



        /*EnemyPawn newEnemy = enemyBuilder.GetObject();
        newEnemy.SetIsIdle(spawnEnemiesIdle);
        newEnemy.Teleport(fixedSpawnPoint);
        pawnsInScene.Add(newEnemy);
        spawnedEnemies++;*/
    }

    public void SpawnEnemyActive()
    {
        EnemyPawn newEnemy = enemyBuilder.GetObject();
        newEnemy.InitializeEnemyPawn(null);
        newEnemy.SetIsIdle(spawnIdle);
        newEnemy.Teleport(GameManager.current.tileService.TileToPos(GetRandomSpawnableTile()));
        pawnsInScene.Add(newEnemy);
        spawnedEnemies++;
    }
    #endregion

    public Pawn GetTarget()
    {
        return GameManager.current.playerPawn;
    }

    public bool IsPlayerClose(Vector3 pos, int range)
    {
        return Vector3.Distance(pos, GameManager.current.playerPawn.transform.position) <= range;
    }

    public void OnEnemyDeath(EnemyPawn e)
    {
        pawnsInScene.Remove(e);
        spawnedEnemies--;
    }

    public void AddPlayablePawn(PlayablePawn pp)
    {
        playablePawnsInScene.Add(pp);
    }

    public void GetAllPlayablePawnsInScene()
    {
        PlayablePawn[] allPp = FindObjectsByType<PlayablePawn>(FindObjectsSortMode.None);
        foreach (PlayablePawn pp in allPp)
        {
            AddPlayablePawn(pp);
        }
    }

    public void InitializeInactivePlayablePawns()
    {
        if (GameManager.current.allPlayablePawnStatBlocks.Count() <= 0) return;

        foreach (PlayablePawn pp in playablePawnsInScene.items)
        {
            if (pp.IsActivePlayer()) continue;
            pp.SetInactivePlayer(GameManager.current.allPlayablePawnStatBlocks[Random.Range(0, GameManager.current.allPlayablePawnStatBlocks.Count())]);
        }
    }

    public PlayablePawn GetClosestPlayablePawn(Vector3 pos, float range)
    {
        if (playablePawnsInScene.Count < 2) return null;

        PlayablePawn result = null;
        float currentResultDistance = range * 2;

        for (int i = 0; i < playablePawnsInScene.Count; i++)
        {
            if (playablePawnsInScene.items[i].IsActivePlayer()) continue;
            float currentPPDistance = Vector3.Distance(playablePawnsInScene.items[i].GetPosition(), pos);
            if (currentPPDistance <= range)
            {
                if (result != null)
                {
                    if (currentPPDistance < currentResultDistance)
                    {
                        currentResultDistance = currentPPDistance;
                        result = playablePawnsInScene.items[i];
                    }
                } else
                {
                    currentResultDistance = currentPPDistance;
                    result = playablePawnsInScene.items[i];
                }
            }
        }

        return result;
    }

    public void SetSpawnVars(float interval, int batch, int max)
    {
        spawnTimerLimit = interval;
        spawnBatch = batch;
        maxSimultaneousEnemies = max;
    }

    private void OnDrawGizmos()
    {
        foreach (Vector3Int v in spawnableTiles)
        {
            Gizmos.DrawCube(GameManager.current.tileService.TileToPos(v), new Vector3(1, 0.3f, 1.4f));
        }
    }
}
