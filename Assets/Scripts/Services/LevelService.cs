using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelService : MonoBehaviour
{
    [SerializeField] List<PO_xp> allXpInScene = new List<PO_xp>();

    [SerializeField] List<InteractableObject> allInteractableObjects = new List<InteractableObject>();
    [SerializeField] List<InteractableObject> allIOAdd = new List<InteractableObject>();
    [SerializeField] List<InteractableObject> allIORemove = new List<InteractableObject>();
    
    [SerializeField] List<InteractableArea> allInteractableAreas = new List<InteractableArea>();

    [SerializeField] private bool isPlayerInsideArea = false;
    InteractableArea currentArea;

    [SerializeField] LevelLogic currentLevelLogic;

    private void OnEnable()
    {
        GameManager.current.eventService.onSpawnXp += SpawnPoXp;
        currentLevelLogic = GameObject.Find("Grid").GetComponent<LevelLogic>();
        GetAllInteractablesInScene();
    }

    private void OnDisable()
    {
        
    }

    #region LevelLogic
    public void StartLevel()
    {
        if (currentLevelLogic == null)
        {
            Debug.Log("Missing Level Logic");
            return;
        }

        if (currentLevelLogic.isActive) return;

        currentLevelLogic.Activate();
    }



    #endregion

    #region XP
    public void SpawnPoXp(Vector3 pos, float value)
    {
        Vector3 newPos = new Vector3(pos.x, 0.25f, pos.z);
        PO_xp newXp = Instantiate(GameManager.current.gameInfo.poXpPrefab, newPos, Quaternion.identity).GetComponent<PO_xp>();
        newXp.Init(value);
        allXpInScene.Add(newXp);
    }

    public void RemoveXpFromList(PO_xp toRemove)
    {
        allXpInScene.Remove(toRemove);
    }
    #endregion

    #region Interactables
    public void GetAllInteractablesInScene()
    {
        InteractableObject[] allIO = FindObjectsByType<InteractableObject>(FindObjectsSortMode.None);
        foreach (InteractableObject io in allIO)
        {
            AddInteractableObject(io);
        }

        InteractableArea[] allIA = FindObjectsByType<InteractableArea>(FindObjectsSortMode.None);
        foreach (InteractableArea ia in allIA)
        {
            AddInteractableArea(ia);
        }
    }


    public InteractableObject GetClosestInteractable(Vector3 pos, float range)
    {
        foreach (InteractableObject io in allIOAdd)
        {
            allInteractableObjects.Add(io);
        }

        allIOAdd.Clear();

        foreach (InteractableObject io in allIORemove)
        {
            allInteractableObjects.Remove(io);
        }

        allIORemove.Clear();


        if (allInteractableObjects.Count < 1) return null;

        InteractableObject result = null;

        for (int i = 0; i < allInteractableObjects.Count; i++)
        {
            if (allInteractableObjects[i].used) continue;
            if (Vector3.Distance(allInteractableObjects[i].transform.position, pos) <= range) result = allInteractableObjects[i];
        }

        return result;
    }

    public bool IsPlayerInsideAnInteractableArea()
    {
        return isPlayerInsideArea;
    }

    public void SetPlayerInsideAnArea(bool isPlayerInside, InteractableArea area)
    {
        GameManager.current.playerPawn.currentArea = area;
        isPlayerInsideArea = isPlayerInside;
    }

    public void SpawnRandomUpgrade(Vector3 pos)
    {
        Upgrade chosen = null;

        int totalLength = GameManager.current.allPawnUpgrades.Length + GameManager.current.allWeaponUpgrades.Length;
        int r = Random.Range(0, totalLength);

        if (r < GameManager.current.allPawnUpgrades.Length)
        {
            chosen = GameManager.current.allPawnUpgrades[r];
            IO_PawnUpgradePickup pUpgrade = Instantiate(GameManager.current.gameInfo.pawnUpgradePrefab, pos, Quaternion.identity).GetComponent<IO_PawnUpgradePickup>();
            pUpgrade.SetUpgrade(chosen as PawnUpgrade);

        } else
        {
            chosen = GameManager.current.allWeaponUpgrades[r - GameManager.current.allPawnUpgrades.Length];
            IO_WeaponUpgradePickup wUpgrade = Instantiate(GameManager.current.gameInfo.weaponUpgradePrefab, pos, Quaternion.identity).GetComponent<IO_WeaponUpgradePickup>();
            wUpgrade.SetUpgrade(chosen as WeaponUpgrade);
        }
    }


    #endregion



    public void AddInteractableObject(InteractableObject io)
    {
        if (allInteractableObjects.Contains(io)) return;
        allIOAdd.Add(io);
    }

    public void RemoveInteractableObject(InteractableObject io)
    {
        if (allInteractableObjects.Contains(io))
        {
            allIORemove.Remove(io);
        }
    }

    public void AddInteractableArea(InteractableArea ia)
    {
        if (allInteractableAreas.Contains(ia)) return;
        allInteractableAreas.Add(ia);
    }

    public void RemoveInteractableArea(InteractableArea ia)
    {
        if (allInteractableAreas.Contains(ia))
        {
            allInteractableAreas.Remove(ia);
        }
    }

}
