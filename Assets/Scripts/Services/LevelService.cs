using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelService : MonoBehaviour
{
    [SerializeField] List<PO_xp> allXpInScene = new List<PO_xp>();

    [SerializeField] List<InteractableObject> allInteractableObjects = new List<InteractableObject>();
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

    #endregion



    public void AddInteractableObject(InteractableObject io)
    {
        if (allInteractableObjects.Contains(io)) return;
        allInteractableObjects.Add(io);
    }

    public void AddInteractableArea(InteractableArea ia)
    {
        if (allInteractableAreas.Contains(ia)) return;
        allInteractableAreas.Add(ia);
    }



}
