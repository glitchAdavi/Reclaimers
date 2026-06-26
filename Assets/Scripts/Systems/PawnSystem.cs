using Reclaimers.UpdateSystem;
using UnityEngine;

namespace Reclaimers.Systems
{
    public class PawnSystem : MonoBehaviour, IUpdateable, IPauseable
    {
        [SerializeField] private CustomList<Pawn> pawnsInScene = new CustomList<Pawn>();

        EnemyBuilder _enemyBuilder;
        GameObject _enemyParent;

        private void Start()
        {
            this.Register();

            EventSystem.Instance.onEnemyDeath += OnEnemyDeath;
            
            _enemyParent = new GameObject("EnemyParent");
            _enemyBuilder = GameManager.Instance.CreateSystem<EnemyBuilder>();
            _enemyBuilder.BuildPool(EnemyFactory, 100, _enemyParent.transform);
        }


        public void ManagedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                SpawnEnemy();
            }
        }

        public void ManagedPause(bool paused)
        {
            
        }




        public void SpawnEnemy(Vector3? pos = null)
        {
            Pawn newEnemy = _enemyBuilder.GetObject();

            pawnsInScene.Add(newEnemy);

            newEnemy.SetAsNonPlayer(GameManager.Instance.gameInfo.defaultEnemyStatBlock, GameManager.Instance.playerPawn);
            if (pos != null) newEnemy.transform.position = (Vector3)pos;
        }

        public void OnEnemyDeath(Pawn enemy)
        {
            pawnsInScene.Remove(enemy);
            _enemyBuilder.ReturnObject(enemy);
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

