using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current { get; private set; }

    public List<Object> allServices = new List<Object>();

    public GameInfo gameInfo;

    public DataPersistenceService dataPersistenceService;
    public TileService tileService;
    public UpdateService updateService;
    public EventService eventService;
    public TimerService timerService;
    public LevelService levelService;
    public PawnService pawnService;
    public UIService uiService;

    public PlayerController playerController;
    public PlayablePawn playerPawn;
    public PlayerCamera playerCamera;

    public ProjectileBuilder projectileBuilder;


    public PawnStatBlock[] allPlayablePawnStatBlocks;
    public PawnUpgrade[] allPawnUpgrades;
    public WeaponUpgrade[] allWeaponUpgrades;
    public WeaponStatBlock[] allWeapons;
    
    //TEMP
    public PawnStatBlock playerStatBlock;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
            
            DontDestroyOnLoad(this);

            gameInfo.useCurrentPlayerStatBlock = false;

            allPlayablePawnStatBlocks = Resources.LoadAll<PawnStatBlock>("ScriptableObjects/StatBlocks/Playable");
            allPawnUpgrades = Resources.LoadAll<PawnUpgrade>("ScriptableObjects/Upgrades");
            allWeaponUpgrades = Resources.LoadAll<WeaponUpgrade>("ScriptableObjects/Upgrades");
            allWeapons = Resources.LoadAll<WeaponStatBlock>("ScriptableObjects/StatBlocks/Weapons");

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        else Destroy(gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        tileService = GameObject.Find("Grid").GetComponent<TileService>();
        eventService = CreateService<EventService>();
        updateService = CreateService<UpdateService>();
        timerService = CreateService<TimerService>();
        levelService = CreateService<LevelService>();
        pawnService = CreateService<PawnService>();
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
        return newUI;
    }



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


    public void GoToLevel(int scenIndex)
    {
        SceneManager.LoadScene(scenIndex);
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
