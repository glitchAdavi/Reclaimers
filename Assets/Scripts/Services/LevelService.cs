using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LevelService : MonoBehaviour
{
    [SerializeField] ExecutableList<PO_xp> allXpInScene = new ExecutableList<PO_xp>();
    [SerializeField] ExecutableList<PO_material> allMaterialsInScene = new ExecutableList<PO_material>();

    [SerializeField] ExecutableList<InteractableObject> allInteractableObjects = new ExecutableList<InteractableObject>();
    [SerializeField] ExecutableList<InteractableArea> allInteractableAreas = new ExecutableList<InteractableArea>();

    [SerializeField] private bool isPlayerInsideArea = false;
    InteractableArea currentArea;

    [SerializeField] LevelLogic currentLevelLogic;

    private void OnEnable()
    {
        GameManager.current.eventService.onSpawnXp += SpawnPoXp;
        GameManager.current.eventService.onSpawnMaterial += SpawnPoMaterial;
        GameManager.current.eventService.onSpawnRandomUpgrade += SpawnRandomUpgrade;
        GameManager.current.eventService.onSpawnPawnUpgrade += SpawnPawnUpgrade;
        GameManager.current.eventService.onSpawnWeaponUpgrade += SpawnWeaponUpgrade;

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

    #region Materials
    public void SpawnPoMaterial(Vector3 pos, float value)
    {
        Vector3 newPos = new Vector3(pos.x, 0.25f, pos.z);
        PO_material newMaterial = Instantiate(GameManager.current.gameInfo.poMaterialPrefab, newPos, Quaternion.identity).GetComponent<PO_material>();
        newMaterial.Init(value);
        allMaterialsInScene.Add(newMaterial);
    }

    public void RemoveMaterialFromList(PO_material toRemove)
    {
        allMaterialsInScene.Remove(toRemove);
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
        if (allInteractableObjects.Count < 1) return null;

        InteractableObject result = null;

        for (int i = 0; i < allInteractableObjects.Count; i++)
        {
            if (allInteractableObjects.items[i].used) continue;
            if (Vector3.Distance(allInteractableObjects.items[i].transform.position, pos) <= range) result = allInteractableObjects.items[i];
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



    public void SpawnPawnUpgrade(Vector3 pos)
    {
        int r = Random.Range(0, GameManager.current.allPawnUpgrades.Count());
        Debug.Log(r);
        PawnUpgrade chosen = GameManager.current.allPawnUpgrades[r];
        Debug.Log(chosen.name);
        IO_PawnUpgradePickup pUpgrade = Instantiate(GameManager.current.gameInfo.pawnUpgradePrefab, pos, Quaternion.identity).GetComponent<IO_PawnUpgradePickup>();
        pUpgrade.SetUpgrade(chosen);
    }

    public void SpawnWeaponUpgrade(Vector3 pos)
    {
        int r = Random.Range(0, GameManager.current.allWeaponUpgrades.Count());
        Debug.Log(r);
        WeaponUpgrade chosen = GameManager.current.allWeaponUpgrades[r];
        Debug.Log(chosen.name);
        IO_WeaponUpgradePickup wUpgrade = Instantiate(GameManager.current.gameInfo.weaponUpgradePrefab, pos, Quaternion.identity).GetComponent<IO_WeaponUpgradePickup>();
        wUpgrade.SetUpgrade(chosen);
    }

    public void SpawnRandomUpgrade(Vector3 pos)
    {
        if (Random.Range(0, 100) < 50)
        {
            SpawnPawnUpgrade(pos);
        } else
        {
            SpawnWeaponUpgrade(pos);
        }
    }


    #endregion



    public void AddInteractableObject(InteractableObject io)
    {
        allInteractableObjects.Add(io);
    }

    public void RemoveInteractableObject(InteractableObject io)
    {
        allInteractableObjects.Remove(io);
    }

    public void AddInteractableArea(InteractableArea ia)
    {
        allInteractableAreas.Add(ia);
    }

    public void RemoveInteractableArea(InteractableArea ia)
    {
        allInteractableAreas.Remove(ia);
    }

}
