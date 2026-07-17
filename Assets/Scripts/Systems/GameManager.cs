using Reclaimers.DataPersistence;
using Reclaimers.DeveloperConsole;
using Reclaimers.Systems;
using Reclaimers.UpdateSystem;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Burst.Intrinsics.X86;

public enum Difficulty
{
    Normal = 10,
    Hard = 15,
    Suicide = 20
}

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager Instance { get; private set; }

    public List<Object> allSystems = new List<Object>();

    public GameInfo gameInfo;

    public DataPersistenceSystem dataPersistenceSystem;

    public UpdateSystem updateSystem;
    public GridSystem gridSystem;
    public EventSystem eventSystem;
    public TimerSystem timerSystem;
    public PawnSystem pawnSystem;
    public LevelSystem levelSystem;

    public UISystem uiSystem;
    public StatCollection statCollection;


    public PlayerController playerController;
    public Pawn playerPawn;
    public StatBlock loadedStatBlock;
    public PlayerCamera playerCamera;

    public CustomList<SpritePart> spriteParts = new CustomList<SpritePart>();
    public CustomList<SpritePart> playerParts = new CustomList<SpritePart>();
    public CustomList<Upgrade> upgradeList = new CustomList<Upgrade>();
    public CustomList<StatusEffect> statusEffectList = new CustomList<StatusEffect>();
    public CustomList<Item> itemList = new CustomList<Item>();
    

    //---AccountData---

    public int accountLevel = 0;
    public float accountXp = 0f;
    public float scaling = 1f;
    public Difficulty selectedDifficulty = Difficulty.Normal;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

            LoadSpriteParts();
            LoadModifiers();

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        else Destroy(gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        TryGetComponent(out dataPersistenceSystem);
        if (dataPersistenceSystem == null) dataPersistenceSystem = CreateSystem<DataPersistenceSystem>(false);

        updateSystem = GetComponent<UpdateSystem>();
        gridSystem = GameObject.Find("Grid").GetComponent<GridSystem>();
        eventSystem = CreateSystem<EventSystem>();
        timerSystem = CreateSystem<TimerSystem>();
        pawnSystem = CreateSystem<PawnSystem>();
        levelSystem = CreateSystem<LevelSystem>();

        uiSystem = InstantiateUI();
        if (SceneManager.GetActiveScene().buildIndex != 0) eventSystem.EnableUITop();
        statCollection = CreateSystem<StatCollection>();

        playerPawn = InstantiatePlayer();
        playerPawn.SetAsPlayer(gameInfo.tempPlayerStatBlock);
        playerCamera = InstantiateCamera();
        playerController = CreateSystem<PlayerController>();
        playerController.AssignPlayerPawn(playerPawn);



        Instance.eventSystem.onSetPause += SetPause;
        Instance.eventSystem.onLevelUpFinish += UpdateScaling;

        uiSystem.FadeIn(levelSystem.StartLevel);
    }

    private void OnSceneUnloaded(Scene current)
    {
        SetPause(false);

        foreach (Object system in allSystems)
        {
            Destroy(system);
        }
        allSystems.Clear();
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

        loadedStatBlock = gameInfo.tempPlayerStatBlock;

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

    public T CreateSystem<T>(bool addToList = true) where T : Component
    {
        T system = gameObject.AddComponent<T>();
        if (addToList) allSystems.Add(system);
        LogSystem.Log($"GameManager: Created " + typeof(T) + ".");
        return system;
    }

    private Pawn InstantiatePlayer()
    {
        Pawn player = Instantiate(gameInfo.pawnPrefab, new Vector3(0f, 0.5633f, 0f), Quaternion.identity)
            .GetComponent<Pawn>();

        return player;
    }

    private PlayerCamera InstantiateCamera()
    {
        PlayerCamera cam = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();

        if (cam == null)
        {
            cam = Instantiate(gameInfo.playerCameraPrefab).GetComponent<PlayerCamera>();
        }

        return cam;
    }

    private UISystem InstantiateUI()
    {
        UISystem newUI = Instantiate(gameInfo.playerUIPrefab, GameObject.Find("Canvas").transform)
                              .GetComponent<UISystem>();
        newUI.gameObject.transform.SetAsFirstSibling();
        return newUI;
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

    #region Modifiers
    public void LoadModifiers()
    {
        upgradeList = new CustomList<Upgrade>(Resources.LoadAll<Upgrade>("ScriptableObjects/Modifiers/Upgrades").ToList());
        LogSystem.Log($"GameManager: Loaded {upgradeList.Count} upgrades.");
        statusEffectList = new CustomList<StatusEffect>(Resources.LoadAll<StatusEffect>("ScriptableObjects/Modifiers/StatusEffects").ToList());
        LogSystem.Log($"GameManager: Loaded {statusEffectList.Count} status effects.");
    }

    public Modifier GetModifier(string id, bool randomQuality = false)
    {
        Modifier result = upgradeList.FirstOrDefault(u => u.modId == id);
        if (result == null) result = statusEffectList.FirstOrDefault(se => se.modId == id);
        if (randomQuality) result.quality = GetRandomQuality();
        return result;
    }

    public Modifier GetRandomModifier()
    {
        int r = Random.Range(1, 11);
        Modifier result;

        result = (Upgrade)upgradeList[Random.Range(0, upgradeList.Count())].Clone();
        result.quality = GetRandomQuality();



        //if (r > 2)
        //{
            //result = (Upgrade)upgradeList[Random.Range(0, upgradeList.Count())].Clone();
            //result.quality = GetRandomQuality();
        //} else
        //{
            //result = (Item)itemList[Random.Range(0, itemList.Count())].Clone();
            //result.quality = GetRandomQuality();
        //}

        return result;
    }


    public Quality GetRandomQuality()
    {
        int r = Random.Range(1, 101);

        if (r <= 50) return Quality.Common;
        else r -= 50;

        if (r <= 25) return Quality.Uncommon;
        else r -= 25;

        if (r <= 15) return Quality.Rare;
        else r -= 15;

        if (r <= 9) return Quality.Epic;
        else r -= 9;

        return Quality.Legendary;
    }

    public Color GetQualityColor(Quality q)
    {
        if (q.Equals(Quality.Common)) return Color.white;
        if (q.Equals(Quality.Uncommon)) return Color.green;
        if (q.Equals(Quality.Rare)) return Color.blue;
        if (q.Equals(Quality.Epic)) return new Color(128, 0, 128);
        return new Color(255, 165, 0);
    }
    #endregion

    public bool IsVisibleToPlayerCamera(Vector3 pos)
    {
        Vector3 posInCamera = playerCamera.gameObject.GetComponent<Camera>().WorldToViewportPoint(pos);

        return posInCamera.x >= -0.05f && posInCamera.x <= 1.05f &&
               posInCamera.y >= -0.05f && posInCamera.y <= 1.05f;
    }

    public void SetPause(bool pause)
    {
        if (pause == updateSystem.isPaused) return;

        updateSystem.TogglePause();
    }

    public void UpdateScaling()
    {
        scaling =  1f + (0.02f * playerPawn.statBlock.level.ValueInt()) * ((int)selectedDifficulty / 10f);
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


