using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Reclaimers.Systems;
using Reclaimers.DataPersistence;
using Reclaimers.DeveloperConsole;
using Reclaimers.UpdateSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager Instance { get; private set; }

    public List<UnityEngine.Object> allSystems = new List<UnityEngine.Object>();

    public GameInfo gameInfo;

    public DataPersistenceSystem dataPersistenceSystem;

    public EventSystem eventSystem;
    public TimerSystem timerSystem;
    public PawnSystem pawnSystem;
    public LevelSystem levelSystem;

    public UISystem uiSystem;


    public PlayerController playerController;
    public Pawn playerPawn;
    public PlayerCamera playerCamera;

    public CustomList<SpritePart> spriteParts = new CustomList<SpritePart>();
    public CustomList<SpritePart> playerParts = new CustomList<SpritePart>();
    

    //---AccountData---

    public int accountLevel = 0;
    public float accountXp = 0f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);

        LoadSpriteParts();

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;


    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        dataPersistenceSystem = CreateSystem<DataPersistenceSystem>();

        eventSystem = CreateSystem<EventSystem>();
        timerSystem = CreateSystem<TimerSystem>();
        pawnSystem = CreateSystem<PawnSystem>();
        levelSystem = CreateSystem<LevelSystem>();

        uiSystem = CreateSystem<UISystem>();

        playerPawn = InstantiatePlayer();
        playerController = CreateSystem<PlayerController>();
        playerController.AssignPlayerPawn(playerPawn);




        timerSystem.SetTimer(1f, levelSystem.StartLevel);
    }

    private void OnSceneUnloaded(Scene current)
    {

    }

    public void LoadData(GameData data)
    {
        accountLevel = data.accountLevel;
        accountXp = data.accountXp;

        foreach (FieldInfo fieldData in typeof(GameData).GetFields())
        {
            FieldInfo fieldStat = typeof(StatBlock).GetField(fieldData.Name);
            if (fieldStat != null)
            {
                fieldStat.SetValue(gameInfo.tempPlayerStatBlock, fieldData.GetValue(data));
            }
        }

        playerPawn.SetAsPlayer(gameInfo.tempPlayerStatBlock);

    }

    public void SaveData(GameData data)
    {
        data.accountLevel = accountLevel;
        data.accountXp = accountXp;

        foreach (FieldInfo fieldStat in typeof(StatBlock).GetFields())
        {
            FieldInfo fieldData = typeof(GameData).GetField(fieldStat.Name);
            if (fieldData != null)
            {
                fieldData.SetValue(data, fieldStat.GetValue(playerPawn.statBlock));
            }
        }
    }

    public T CreateSystem<T>() where T : Component
    {
        T system = gameObject.AddComponent<T>();
        allSystems.Add(system);
        LogSystem.Log($"GameManager: Created " + typeof(T) + ".");
        return system;
    }

    private Pawn InstantiatePlayer()
    {
        Pawn player = Instantiate(gameInfo.pawnPrefab, new Vector3(0f, 0.5633f, 0f), Quaternion.identity)
            .GetComponent<Pawn>();

        player.SetAsPlayer(gameInfo.defaultPlayerStatBlock);

        return player;
    }






    public float DistanceToPlayer(Vector3 pos)
    {
        return Vector3.Distance(playerPawn.transform.position, pos);
    }

    public void LoadSpriteParts()
    {
        spriteParts = new CustomList<SpritePart>(Resources.LoadAll<SpritePart>("ScriptableObjects/SpriteParts").ToList());

        foreach  (SpritePart sp in spriteParts)
        {
            if (sp.spCategory.Equals(SpritePartCategory.Player)) playerParts.Add(sp);
        }
    }

    public SpritePart GetSpritePart(string partName)
    {
        return spriteParts.FirstOrDefault(sp => sp.partName == partName);
    }

    public bool IsVisibleToPlayerCamera(Vector3 pos)
    {
        Vector3 posInCamera = playerCamera.gameObject.GetComponent<Camera>().WorldToViewportPoint(pos);

        return posInCamera.x >= -0.05f && posInCamera.x <= 1.05f &&
               posInCamera.y >= -0.05f && posInCamera.y <= 1.05f;
    }
}


