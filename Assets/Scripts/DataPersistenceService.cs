using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceService : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;


    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceService Instance { get; private set; }

    public void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    public void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    public void NewGame()
    {
        Debug.Log("Initialize New Game");

        this.gameData = new GameData();
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null) NewGame();

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public void SetFileName(string newFileName)
    {
        fileName = newFileName;
    }
}
