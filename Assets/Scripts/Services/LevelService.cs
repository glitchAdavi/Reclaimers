using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelService : MonoBehaviour
{
    [SerializeField] List<PO_xp> allXpInScene = new List<PO_xp>();

    [SerializeField] List<InteractableObject> allinteractableObjects = new List<InteractableObject>();

    [SerializeField] LevelLogic currentLevelLogic;

    private void OnEnable()
    {
        GameManager.current.eventService.onSpawnXp += SpawnPoXp;
        currentLevelLogic = gameObject.GetComponent<LevelLogic>();
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
    public InteractableObject GetClosestInteractable(Vector3 pos, float range)
    {
        if (allinteractableObjects.Count < 1) return null;

        InteractableObject result = null;

        for (int i = 0; i < allinteractableObjects.Count; i++)
        {
            if (Vector3.Distance(allinteractableObjects[i].transform.position, pos) <= range) result = allinteractableObjects[i];
        }

        return result;
    }

    #endregion

}
