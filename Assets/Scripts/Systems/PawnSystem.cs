using Reclaimers.DeveloperConsole;
using Reclaimers.UpdateSystem;
using UnityEngine;

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

        EnemyBuilder _enemyBuilder;
        GameObject _enemyParent;

        [SerializeField] private CustomList<Pawn> pawnsInScene = new CustomList<Pawn>();
        [SerializeField] private CustomList<Corpse> corpsesInScene = new CustomList<Corpse>();
        public int maxCorpsesInScene = 150;

        public CustomList<Vector3> mapSpawnPoints = new CustomList<Vector3>();
        public float mapSpawnPointRange = 20f;

        private CustomList<Vector3Int> spawnableTiles = new CustomList<Vector3Int>();
        public Vector3Int spawneableArea = new Vector3Int(20, 14, 3);


        public float spawnTime = 1f;
        public int spawnBatch = 1;
        public int maxEnemiesInPlay = 30;
        public int enemiesInPlay = 0;

        public int permanentMaxEnemiesInPlay = 300;

        public CustomList<SpawnBlock> enemiesToSpawn = new CustomList<SpawnBlock>();
        private int totalWeight = 0;

        public bool canSpawn = false;
        public bool enemiesSpawnAlert = false;
        public bool enemiesSpawnIdle = false;

        public bool isPaused = false;

        private void Start()
        {
            this.Register();

            EventSystem.Instance.onEnemyDeath += OnEnemyDeath;
            
            _enemyParent = new GameObject("EnemyParent");
            _enemyBuilder = GameManager.Instance.CreateSystem<EnemyBuilder>();
            _enemyBuilder.BuildPool(EnemyFactory, 100, _enemyParent.transform);

            Transform[] currentMapSpawnPoints = GameObject.Find("MapSpawnPoints").GetComponentsInChildren<Transform>();
            foreach (Transform t in currentMapSpawnPoints)
            {
                if (t.position.Equals(Vector3.zero)) continue;
                mapSpawnPoints.Add(t.position);
            }
        }


        public void ManagedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                SpawnEnemy(GameManager.Instance.gameInfo.defaultEnemyStatBlock, new Vector3(-5f, 0.25f, -5f), 0, false);
            }
        }

        public void ManagedPause(bool paused)
        {
            
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

            if (enemiesSpawnIdle)
            {
                for (int i = 0; i < mapSpawnPoints.Count; i++)
                {
                    for (int j = 0; j < spawnBatch; j++)
                    {
                        SpawnEnemy(SelectEnemy(), mapSpawnPoints[i], -1, true);
                    }
                }
                enemiesSpawnIdle = false;
            }

            if (enemiesSpawnAlert)
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
            newEnemy.SetAsNonPlayer(enemy, GameManager.Instance.playerPawn);
            //newEnemy.SetIsIdle(idle);
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
            pawnsInScene.Add(newEnemy);


            /*
            Pawn newEnemy = _enemyBuilder.GetObject();

            pawnsInScene.Add(newEnemy);

            newEnemy.SetAsNonPlayer(GameManager.Instance.gameInfo.defaultEnemyStatBlock, GameManager.Instance.playerPawn);
            if (pos != null) newEnemy.transform.position = (Vector3)pos;
            */
        }





        #endregion

        public void OnEnemyDeath(Pawn enemy)
        {
            pawnsInScene.Remove(enemy);
            _enemyBuilder.ReturnObject(enemy);
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
    }
}

