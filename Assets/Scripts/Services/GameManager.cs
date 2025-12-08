using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current { get; private set; }

    public List<Object> allServices = new List<Object>();

    public GameInfo gameInfo;

    public DeveloperConsole console;
    public DataPersistenceService dataPersistenceService;
    public TileService tileService;
    public UpdateService updateService;
    public EventService eventService;
    public TimerService timerService;
    public AudioService audioService;
    public LevelService levelService;
    public PawnService pawnService;
    public StatService statService;
    public UIService uiService;

    public PlayerController playerController;
    public PlayablePawn playerPawn;
    public PlayerCamera playerCamera;

    public ProjectileBuilder projectileBuilder;


    public List<PawnStatBlock> allBosses = new List<PawnStatBlock>();
    public List<PawnStatBlock> allPlayablePawns = new List<PawnStatBlock>();
    public List<WeaponStatBlock> allWeapons = new List<WeaponStatBlock>();
    public List<AbilityStatBlock> allAbilities = new List<AbilityStatBlock>();
    public List<PawnUpgrade> allPawnUpgrades = new List<PawnUpgrade>();
    public List<WeaponUpgrade> allWeaponUpgrades = new List<WeaponUpgrade>();
    public List<AbilityUpgrade> allAbilityUpgrades = new List<AbilityUpgrade>();
    public List<Modifier> allModifiers = new List<Modifier>();
    
    //TEMP for the case in which the player quits and we have to reset fully
    public PawnStatBlock playerStatBlock;


    private void Awake()
    {
        if (current == null)
        {
            current = this;
            
            DontDestroyOnLoad(this);

            gameInfo.useCurrentPlayerStatBlock = false;

            allBosses = Resources.LoadAll<PawnStatBlock>("ScriptableObjects/StatBlocks/Boss").ToList();
            allPlayablePawns = Resources.LoadAll<PawnStatBlock>("ScriptableObjects/StatBlocks/Playable").ToList();
            allWeapons = Resources.LoadAll<WeaponStatBlock>("ScriptableObjects/StatBlocks/Weapons").ToList();
            allAbilities = Resources.LoadAll<AbilityStatBlock>("ScriptableObjects/StatBlocks/Abilities").ToList();
            allPawnUpgrades = Resources.LoadAll<PawnUpgrade>("ScriptableObjects/Upgrades/Pawn").ToList();
            allWeaponUpgrades = Resources.LoadAll<WeaponUpgrade>("ScriptableObjects/Upgrades/Weapon").ToList();
            allAbilityUpgrades = Resources.LoadAll<AbilityUpgrade>("ScriptableObjects/Upgrades/Ability").ToList();
            allModifiers = Resources.LoadAll<Modifier>("ScriptableObjects/Modifiers").ToList();

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        else Destroy(gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        console = CreateService<DeveloperConsole>();
        tileService = GameObject.Find("Grid").GetComponent<TileService>();
        eventService = CreateService<EventService>();
        updateService = CreateService<UpdateService>();
        timerService = CreateService<TimerService>();
        audioService = CreateService<AudioService>();
        levelService = CreateService<LevelService>();
        pawnService = CreateService<PawnService>();
        statService = CreateService<StatService>();
        uiService = InstantiateUI();


        projectileBuilder = CreateService<ProjectileBuilder>();

        playerController = CreateService<PlayerController>();
        playerPawn = InstantiatePlayer();
        playerCamera = InstantiateCamera();


        //dataPersistenceService = CreateService<DataPersistenceService>();
        //dataPersistenceService.SetFileName("data");
        //dataPersistenceService.LoadGame();


        // Start level after everything else is assigned
        uiService.FadeIn(levelService.StartLevel);
    }

    private void OnSceneUnloaded(Scene current)
    {
        for (int i = allServices.Count - 1; i >= 0; i--)
        {
            Destroy(allServices[i]);
        }

        allServices.Clear();
    }

    public T CreateService<T>() where T : Component
    {
        T service = gameObject.AddComponent<T>();
        allServices.Add(service);
        return service;
    }

    private PlayablePawn InstantiatePlayer()
    {
        PlayablePawn newPawn = Instantiate(gameInfo.playablePawnPrefab, new Vector3(0, 0.5633f, 0), Quaternion.identity)
                              .GetComponent<PlayablePawn>();

        if (gameInfo.useCurrentPlayerStatBlock) newPawn.SetActivePlayer(gameInfo.currentPlayerStatBlock);
        else newPawn.SetActivePlayer(null);
        gameInfo.playerPositionVar.SetValue(newPawn.GetPosition());
        pawnService.AddPlayablePawn(newPawn);

        playerController?.AssignPlayerPawn(newPawn);
        return newPawn;
    }

    private PlayerCamera InstantiateCamera()
    {
        PlayerCamera newCamera = Instantiate(gameInfo.playerCameraPrefab, new Vector3(0, 0, 0), gameInfo.playerCameraPrefab.transform.rotation)
                                .GetComponent<PlayerCamera>();
        return newCamera;
    }

    private UIService InstantiateUI()
    {
        UIService newUI = Instantiate(gameInfo.playerUIPrefab, GameObject.Find("Canvas").transform)
                              .GetComponent<UIService>();
        newUI.gameObject.transform.SetAsFirstSibling();
        return newUI;
    }


    #region Player
    public void SetNewActivePlayer(PlayablePawn newPlayer)
    {
        Debug.Log($"{newPlayer}");
        PlayablePawn playerToInactivate = playerPawn;
        newPlayer.SetActivePlayer();
        playerController?.AssignPlayerPawn(newPlayer);
        playerPawn = newPlayer;
        playerToInactivate.SetInactivePlayer();
    }

    public void SetNewProjectile(GameObject newProj)
    {
        projectileBuilder.NewPool(newProj.GetComponent<Projectile>(), 100);
    }

    public void SavePlayer()
    {

        playerPawn.SaveUpgradeDictionary();
        gameInfo.currentPlayerStatBlock.CopyValues(playerPawn.statBlock);
        gameInfo.useCurrentPlayerStatBlock = true;
    }

    public void ResetPlayer()
    {
        gameInfo.currentPlayerStatBlock.CopyValues(gameInfo.defaultStatBlock);
        gameInfo.useCurrentPlayerStatBlock = false;
    }
    #endregion

    #region GetRandom
    public PawnStatBlock GetRandomBoss()
    {
        return allBosses[Random.Range(0, allBosses.Count())];
    }

    public PawnStatBlock GetRandomPlayableStatBlock()
    {
        List<PawnStatBlock> filteredList = allPlayablePawns.Where(x => x.rarity.Equals(GetRarity())).ToList();
        return filteredList[Random.Range(0, filteredList.Count())];
    }

    public WeaponStatBlock GetRandomWeaponStatBlock()
    {
        List<WeaponStatBlock> filteredList = allWeapons.Where(x => x.rarity.Equals(GetRarity())).ToList();
        return filteredList[Random.Range(0, filteredList.Count())];
    }

    public WeaponStatBlock GetWeaponStatBlockByName(string name)
    {
        WeaponStatBlock result = null;
        foreach(WeaponStatBlock wsb in allWeapons)
        {
            if (wsb.internalName.Equals(name)) result = wsb;
        }
        return result;
    }

    public AbilityStatBlock GetRandomAbilityStatBlock()
    {
        List<AbilityStatBlock> filteredList = allAbilities.Where(x => x.rarity.Equals(GetRarity())).ToList();
        return filteredList[Random.Range(0, filteredList.Count())];
    }

    public AbilityStatBlock GetAbilityStatBlockByName(string name)
    {
        AbilityStatBlock result = null;
        foreach (AbilityStatBlock asb in allAbilities)
        {
            if (asb.internalName.Equals(name)) result = asb;
        }
        return result;
    }

    public Upgrade GetRandomUpgrade()
    {
        Upgrade result = null;
        while (result == null)
        {
            int r = Random.Range(0, 3);
            switch (r)
            {
                case 0:
                    result = GetRandomPawnUpgrade();
                    break;
                case 1:
                    result = GetRandomWeaponUpgrade();
                    break;
                case 2:
                    result = GetRandomAbilityUpgrade();
                    break;
                default:
                    result = null;
                    break;
            }
        }
        return result;
        
    }

    public PawnUpgrade GetRandomPawnUpgrade()
    {
        if (allPawnUpgrades.Count < 1) return null;
        List<PawnUpgrade> filteredList = allPawnUpgrades.Where(x => x.rarity.Equals(GetRarity())).ToList();
        if (filteredList.Count < 1) return null;
        return filteredList[Random.Range(0, filteredList.Count())];
    }

    public WeaponUpgrade GetRandomWeaponUpgrade()
    {
        if (allWeaponUpgrades.Count < 1) return null;
        List<WeaponUpgrade> filteredList = allWeaponUpgrades.Where(x => x.rarity.Equals(GetRarity())).ToList();
        if (filteredList.Count < 1) return null;
        return filteredList[Random.Range(0, filteredList.Count())];
    }

    public AbilityUpgrade GetRandomAbilityUpgrade()
    {
        if (allAbilityUpgrades.Count < 1) return null;
        List<AbilityUpgrade> filteredList = allAbilityUpgrades.Where(x => x.rarity.Equals(GetRarity())).ToList();
        if (filteredList.Count < 1) return null;
        return filteredList[Random.Range(0, filteredList.Count())];
    }

    public Upgrade GetUpgradeByName(string name)
    {
        Upgrade result = null;
        foreach (PawnUpgrade pu in allPawnUpgrades)
        {
            if (pu.internalName.Equals(name)) result = pu;
        }
        foreach (WeaponUpgrade wu in allWeaponUpgrades)
        {
            if (wu.internalName.Equals(name)) result = wu;
        }
        foreach (AbilityUpgrade au in allAbilityUpgrades)
        {
            if (au.internalName.Equals(name)) result = au;
        }
        return result;
    }

    public Rarity GetRarity()
    {
        int r = Random.Range(1, 101);

        if (r >= (int)Rarity.Common) return Rarity.Common;
        if (r >= (int)Rarity.Uncommon) return Rarity.Uncommon;
        if (r >= (int)Rarity.Rare) return Rarity.Rare;
        if (r >= (int)Rarity.Epic) return Rarity.Epic;
        return Rarity.Legendary;
    }
    #endregion

    public float DistanceToPlayer(Vector3 pos)
    {
        return Vector3.Distance(playerPawn.transform.position, pos);
    }

    public Modifier GetModifier(string id)
    {
        foreach (Modifier mod in allModifiers)
        {
            if (mod.id.Equals(id)) return mod;
        }
        return null;
    }

    public void GoToLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public float Scaling()
    {
        return 1f + (0.05f * playerPawn.GetLevel());
    }

    public void ReturnToMenu()
    {
        GoToLevel(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
