using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current { get; private set; }

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
    

    //TEMP
    public PawnStatBlock playerStatBlock;

    private void Awake()
    {
        if (current == null) current = this;
        else Destroy(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
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

        playerController = CreateService<PlayerController>();
        playerPawn = InstantiatePlayer();
        playerCamera = InstantiateCamera();

        projectileBuilder = CreateService<ProjectileBuilder>();


        dataPersistenceService = CreateService<DataPersistenceService>();
        dataPersistenceService.SetFileName("data");
        //dataPersistenceService.LoadGame();
    }

    private void OnSceneUnloaded(Scene current)
    {
        tileService = null;
        Destroy(eventService);
        eventService = null;
        Destroy(updateService);
        updateService = null;
        Destroy(timerService);
        timerService = null;
        Destroy(levelService);
        levelService = null;
        Destroy(pawnService);
        pawnService = null;

        playerController = null;
        playerPawn = null;
        playerCamera = null;

        Destroy(projectileBuilder);
        projectileBuilder = null;
    }



    public T CreateService<T>() where T : Component
    {
        T service = gameObject.AddComponent<T>();
        return service;
    }

    private PlayablePawn InstantiatePlayer()
    {
        PlayablePawn newPawn = Instantiate(gameInfo.playablePawnPrefab, new Vector3(0, 0, 0), Quaternion.identity)
                              .GetComponent<PlayablePawn>();
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
}
