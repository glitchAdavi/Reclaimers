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

    public UpdateSystem updateSystem;
    public GridSystem gridSystem;
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
    public CustomList<Upgrade> upgradeList = new CustomList<Upgrade>();
    public CustomList<StatusEffect> statusEffectList = new CustomList<StatusEffect>();
    

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
        LoadModifiers();

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;


    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        dataPersistenceSystem = CreateSystem<DataPersistenceSystem>();

        updateSystem = GetComponent<UpdateSystem>();
        gridSystem = GameObject.Find("Grid").GetComponent<GridSystem>();
        eventSystem = CreateSystem<EventSystem>();
        timerSystem = CreateSystem<TimerSystem>();
        pawnSystem = CreateSystem<PawnSystem>();
        levelSystem = CreateSystem<LevelSystem>();

        uiSystem = CreateSystem<UISystem>();

        playerPawn = InstantiatePlayer();
        playerController = CreateSystem<PlayerController>();
        playerController.AssignPlayerPawn(playerPawn);



        EventSystem.Instance.onSetPause += SetPause;

        timerSystem.SetTimer(1f, levelSystem.StartLevel);
    }

    private void OnSceneUnloaded(Scene current)
    {

    }

    public void LoadData(GameData data)
    {
        accountLevel = data.accountLevel;
        accountXp = data.accountXp;

        foreach (FieldInfo dataField in typeof(GameData).GetFields())
        {
            FieldInfo statField = typeof(StatBlock).GetField(dataField.Name);
            if (statField != null)
            {
                statField.SetValue(gameInfo.tempPlayerStatBlock, dataField.GetValue(data));
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

        //player.SetAsPlayer(gameInfo.defaultPlayerStatBlock);

        return player;
    }






    public float DistanceToPlayer(Vector3 pos)
    {
        return Vector3.Distance(playerPawn.transform.position, pos);
    }

    public void LoadSpriteParts()
    {
        spriteParts = new CustomList<SpritePart>(Resources.LoadAll<SpritePart>("ScriptableObjects/SpriteParts").ToList());
        LogSystem.Log($"GameManager: Loaded {spriteParts.Count} sprite parts.");

        foreach  (SpritePart sp in spriteParts)
        {
            if (sp.spCategory.Equals(SpritePartCategory.Player)) playerParts.Add(sp);
        }
    }

    public SpritePart GetSpritePart(string partName)
    {
        return spriteParts.FirstOrDefault(sp => sp.partName == partName);
    }

    public void LoadModifiers()
    {
        upgradeList = new CustomList<Upgrade>(Resources.LoadAll<Upgrade>("ScriptableObjects/Modifiers/Upgrades").ToList());
        LogSystem.Log($"GameManager: Loaded {upgradeList.Count} upgrades.");
        statusEffectList = new CustomList<StatusEffect>(Resources.LoadAll<StatusEffect>("ScriptableObjects/Modifiers/StatusEffects").ToList());
        LogSystem.Log($"GameManager: Loaded {statusEffectList.Count} status effects.");
    }

    public Modifier GetModifier(string id)
    {
        Modifier result = upgradeList.FirstOrDefault(u => u.modId == id);
        if (result == null) result = statusEffectList.FirstOrDefault(se => se.modId == id);
        return result;
    }

    public bool IsVisibleToPlayerCamera(Vector3 pos)
    {
        Vector3 posInCamera = playerCamera.gameObject.GetComponent<Camera>().WorldToViewportPoint(pos);

        return posInCamera.x >= -0.05f && posInCamera.x <= 1.05f &&
               posInCamera.y >= -0.05f && posInCamera.y <= 1.05f;
    }

    public void SetPause(bool pause)
    {
        if (pause && updateSystem.isPaused) return;
        if (!pause && !updateSystem.isPaused) return;

        updateSystem.TogglePause();
    }

    public float Scaling()
    {
        return 1f + (0.02f * playerPawn.statBlock.level.ValueInt());
    }

    public void GoToLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}


