using Reclaimers.DeveloperConsole;
using Reclaimers.UpdateSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Reclaimers.Systems
{
    public struct SpawnBlock
    {
        public StatBlock pawn;
        public int weight;

        public SpawnBlock(StatBlock p, int w)
        {
            pawn = p;
            weight = w;
        }
    }



    public class PawnSystem : MonoBehaviour, IUpdateable, IPauseable
    {

        ObjectBuilder<Pawn> _enemyBuilder;
        GameObject _enemyParent;

        [SerializeField] private CustomList<Pawn> pawnsInScene = new CustomList<Pawn>();
        [SerializeField] private CustomList<Corpse> corpsesInScene = new CustomList<Corpse>();
        public int maxCorpsesInScene = 150;

        public CustomList<Vector3> mapSpawnPoints = new CustomList<Vector3>();
        public float mapSpawnPointRange = 20f;

        private CustomList<Vector3Int> spawnableTiles = new CustomList<Vector3Int>();
        public Vector3Int spawneableArea = new Vector3Int(16, 9, 3);


        public float spawnTime = 1f;
        public int spawnBatch = 1;
        public int maxEnemiesInPlay = 30;
        public int enemiesInPlay = 0;

        public int permanentMaxEnemiesInPlay = 300;

        public CustomList<SpawnBlock> enemiesToSpawn = new CustomList<SpawnBlock>();
        private int totalWeight = 0;

        public bool canSpawn = false;
        public bool populateLevel = false;

        public bool isPaused = false;
        public bool showGizmos = false;

        Timer spawnTimer;

        private void Start()
        {
            this.Register();
            
            _enemyParent = new GameObject("EnemyParent");
            _enemyBuilder = new ObjectBuilder<Pawn>();
            _enemyBuilder.BuildPool(EnemyFactory, 100, _enemyParent.transform);

            Transform[] playerSpawnPoints = GameObject.Find("PlayerSpawnPoints").GetComponentsInChildren<Transform>();
            foreach (Transform t in playerSpawnPoints)
            {
                if (t.position.Equals(Vector3.zero)) continue;
                mapSpawnPoints.Add(t.position);
            }

            spawnTimer = GameManager.Instance.timerSystem.SetForeverTimer(spawnTime, TrySpawn, "SYSTEM_PAWN_Spawn");

            AddSpawns(GameManager.Instance.gameInfo.slimeEnemyStatBlock, 100);

            GameManager.Instance.eventSystem.onEnemyDeath += OnEnemyDeath;
            GameManager.Instance.eventSystem.onToggleSpawn += (active) => canSpawn = active;
            GameManager.Instance.eventSystem.onPopulateLevel += () => populateLevel = true;
            GameManager.Instance.eventSystem.onRequestAddPawnSpawn += AddSpawns;
            GameManager.Instance.eventSystem.onRequestClearPawnSpawns += ClearSpawns;
            GameManager.Instance.eventSystem.onSetPawnSpawnVars += SetSpawnVars;
            GameManager.Instance.eventSystem.onRequestBossSpawn += SpawnBoss;
        }


        public void ManagedUpdate()
        {
            GetSpawnableTiles();

            if (Input.GetKeyDown(KeyCode.I))
            {
                SpawnEnemy(GameManager.Instance.gameInfo.defaultEnemyStatBlock, new Vector3(-10f, 0.25f, -10f), 0, false);
            }
        }

        public void ManagedPause(bool paused)
        {
            isPaused = paused;

            spawnTimer?.Pause(isPaused);
        }



        #region Spawn
        public void GetSpawnableTiles()
        {
            Vector3 playerPos = GameManager.Instance.playerPawn.transform.position;
            Vector3 spawnPosCenter = new Vector3(playerPos.x, 0f, playerPos.z);
            spawnableTiles = GameManager.Instance.gridSystem.GetAllTilesInRangeFromPos(spawnPosCenter, spawneableArea.x, spawneableArea.y, spawneableArea.z);
        }

        public Vector3 GetRandomSpawnableTile()
        {
            return GameManager.Instance.gridSystem.TileToPos(spawnableTiles[Random.Range(0, spawnableTiles.Count)]);
        }

        public void TrySpawn()
        {
            if (!canSpawn || isPaused) return;

            if (populateLevel)
            {
                LogSystem.Log("PawnSystem: populating level.");
                for (int i = 0; i < mapSpawnPoints.Count; i++)
                {
                    for (int j = 0; j < spawnBatch; j++)
                    {
                        SpawnEnemy(SelectEnemy(), mapSpawnPoints[i], -1, true);
                    }
                }
                canSpawn = false;
                populateLevel = false;
            } else
            {
                for (int i = 0; i < spawnBatch; i++)
                {
                    if (enemiesInPlay >= maxEnemiesInPlay) break;
                    SpawnEnemy(SelectEnemy());
                }
            }
        }

        public StatBlock SelectEnemy()
        {
            if (enemiesToSpawn.Count < 1) return null;

            int setWeight = Random.Range(0, totalWeight + 1);
            int partialWeight = 0;
            foreach (SpawnBlock sBlock in enemiesToSpawn)
            {
                partialWeight += sBlock.weight;
                if (setWeight <= partialWeight)
                {
                    return sBlock.pawn;
                }
            }
            return null;
        }

        public void SpawnEnemy(StatBlock enemy, Vector3? pos = null, float radius = -1, bool idle = false)
        {
            if (enemy == null)
            {
                LogSystem.Log("PawnSystem: trying to spawn NULL statBlock");
                return;
            }

            Pawn newEnemy = _enemyBuilder.GetObject();
            newEnemy.SetAsNonPlayer(enemy, idle ? null : GameManager.Instance.playerPawn);
            if (pos.HasValue)
            {
                Vector3 position = (Vector3)pos;
                if (radius < 0) newEnemy.Teleport(GetRandomPosInRadius(position, mapSpawnPointRange));
                else newEnemy.Teleport(GetRandomPosInRadius(position, radius));
            } else
            {
                newEnemy.Teleport(GetRandomSpawnableTile());
                enemiesInPlay++;
            }
            if (idle || enemy.spawnsIdle) newEnemy.SetIdle();
            pawnsInScene.Add(newEnemy);
        }



        public void SetSpawnList(CustomList<SpawnBlock> enemies)
        {
            ClearSpawns();
            foreach (SpawnBlock sb in enemies)
            {
                totalWeight += sb.weight;
            }
            enemiesToSpawn = enemies;
        }

        public void AddSpawns(StatBlock enemy, int weight)
        {
            totalWeight += weight;
            enemiesToSpawn.Add(new SpawnBlock(enemy, weight));
        }

        public void ClearSpawns()
        {
            enemiesToSpawn.Clear();
            totalWeight = 0;
        }
        #endregion

        public void ResetEnemy(Vector3 pos, Pawn e)
        {
            int aux = Random.Range(0, 2);
            if (aux == 0) TryReset(e.gameObject, FindNearestTile(pos));
            else TryReset(e.gameObject, GetRandomSpawnableTile());
        }

        public bool TryReset(GameObject e, Vector3 tile)
        {
            if (NavMesh.SamplePosition(tile, out var _, 1f, 1))
            {
                e.GetComponent<Pawn>().Teleport(tile);
                return true;
            }
            return false;
        }

        public Vector3 FindNearestTile(Vector3 point)
        {
            Vector3 result = Vector3.zero;
            float num = 1000f;
            point = new Vector3(0f - point.x, 0f, 0f - point.z);
            point += GameManager.Instance.playerPawn.transform.position;
            foreach (Vector3Int tile in spawnableTiles)
            {
                Vector3 point2 = GameManager.Instance.gridSystem.TileToPos(tile);
                if (CheckRange(point, point2, num))
                {
                    num = Vector3.Distance(point, point2);
                    result = point2;
                }
            }
            return result;
        }

        public bool CheckRange(Vector3 pos1, Vector3 pos2, float range)
        {
            float maxRangeSqr = range * range;
            Vector3 dir = pos2 - pos1;
            return dir.sqrMagnitude <= maxRangeSqr;
        }

        public bool IsOutsidePlayerRange(Vector3 pos)
        {
            Vector3Int tile = GameManager.Instance.gridSystem.PosToTile(pos);
            Vector3Int distance = DistanceInTiles(GameManager.Instance.gridSystem.GetPawnTilePos(GameManager.Instance.playerPawn), tile);
            if (distance.x > spawneableArea.x + spawneableArea.z + 3 || distance.y > spawneableArea.y + spawneableArea.z + 3)
            {
                return true;
            }
            return false;
        }

        public Vector3Int DistanceInTiles(Vector3Int center, Vector3Int pos)
        {
            Vector3Int vector3Int = center - pos;
            return new Vector3Int(Mathf.Abs(vector3Int.x), Mathf.Abs(vector3Int.y), Mathf.Abs(vector3Int.z));
        }

        public void OnEnemyDeath(Pawn enemy)
        {
            pawnsInScene.Remove(enemy);
            _enemyBuilder.ReturnObject(enemy);
            enemiesInPlay--;
        }

        private Vector3 GetRandomPosInRadius(Vector3 pos, float radius)
        {
            Vector3 randomPos = new Vector3(0f, -100f, 0f);

            while (!GameManager.Instance.gridSystem.SamplePosition(randomPos))
            {
                randomPos = new Vector3(Random.Range(-radius / 2, radius / 2),
                                            0f,
                                            Random.Range(-radius / 2, radius / 2));
                randomPos += pos;
            }

            return randomPos;
        }


        public Pawn EnemyFactory(Transform parent = null)
        {
            Pawn newPawn = Instantiate(GameManager.Instance.gameInfo.pawnPrefab, new Vector3(-15f, 0f, -15f), Quaternion.identity, parent)
                .GetComponent<Pawn>();
            newPawn.SetAsNonPlayer(GameManager.Instance.gameInfo.defaultEnemyStatBlock, GameManager.Instance.playerPawn);

            return newPawn;
        }

        public void SetSpawnVars(float interval, int batch, int max)
        {
            spawnTimer?.Cancel();
            spawnTime = interval;
            spawnTimer = GameManager.Instance.timerSystem.SetForeverTimer(spawnTime, TrySpawn);
            maxEnemiesInPlay = (max > permanentMaxEnemiesInPlay) ? permanentMaxEnemiesInPlay : max;
            spawnBatch = (batch > maxEnemiesInPlay) ? maxEnemiesInPlay : batch;
        }

        public void SpawnBoss()
        {
            LogSystem.Log("Spawn Boss");
        }





        private void OnDrawGizmos()
        {
            if (showGizmos)
            {
                foreach (Vector3Int v in spawnableTiles)
                {
                    Gizmos.DrawCube(GameManager.Instance.gridSystem.TileToPos(v), new Vector3(1, 0.3f, 1.4f));
                }

                foreach (Vector3 v in mapSpawnPoints)
                {
                    Gizmos.DrawWireSphere(v, mapSpawnPointRange / 2);
                }
            }
        }
    }
}

