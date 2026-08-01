using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Reclaimers.DeveloperConsole;

namespace Reclaimers.DataPersistence
{
    public class DataPersistenceSystem : MonoBehaviour
    {
        [Header("File Storage Config")]
        [SerializeField] private string fileName = "persistent.json";

        private GameData gameData;
        private List<IDataPersistence> dataPersistenceObjects;
        private FileDataHandler dataHandler;

        public static DataPersistenceSystem Instance { get; private set; }

        public bool hasLoaded = false;


        private void Awake()
        {
            if (Instance != null) Destroy(this);
            else Instance = this;
        }

        private void Start()
        {
            dataHandler = new FileDataHandler(Application.dataPath, fileName);
            dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        public void NewGame()
        {
            gameData = new GameData();
        }

        public void LoadGame()
        {
            gameData = dataHandler.Load();

            if (gameData == null)
            {
                LogSystem.Log("No data was found. Initializing data to defaults.");
                NewGame();
            }

            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }

            hasLoaded = true;
            GameManager.Instance.eventSystem.FinishLoading();
        }

        public void SaveGame()
        {
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(gameData);
            }

            dataHandler.Save(gameData);
        }

        private void OnApplicationQuit()
        {
            //SaveGame();
        }

        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include ,FindObjectsSortMode.None).OfType<IDataPersistence>();
            return new List<IDataPersistence>(dataPersistenceObjects);
        }
    }
}

