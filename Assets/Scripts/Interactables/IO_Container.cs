using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IO_Container : InteractableObject
{
    public float lootSpreadRange = 5f;

    public Sprite closed;
    public Sprite opened;

    public Vector2Int xpMinMax = new Vector2Int(5, 10);
    public Vector2Int materialMinMax = new Vector2Int(0, 5);
    public Vector2Int upgradeMinMax = new Vector2Int(1, 4);
    public Vector2Int weaponPickupMinMax = new Vector2Int(0, 0);


    private void Awake()
    {
        useThreshold = 0.5f;
    }

    private Vector3 GetRandomPosInRadius()
    {
        Vector3 randomPos = new Vector3(Random.Range(-lootSpreadRange / 2, lootSpreadRange / 2),
                                        0f,
                                        Random.Range(-lootSpreadRange / 2, lootSpreadRange / 2));

        if (randomPos.x < 0) randomPos.x -= transform.localScale.x / 2;
        else randomPos.x += transform.localScale.x / 2;

        if (randomPos.z < 0) randomPos.z -= transform.localScale.z / 2;
        else randomPos.z += transform.localScale.z / 2;

        return randomPos + transform.position;
    }


    protected override void OnFinishEffect()
    {
        int xpRange = Random.Range(xpMinMax.x, xpMinMax.y + 1);
        for (int i = 0; i < xpRange; i++)
        {
            GameManager.current.eventService.SpawnXp(GetRandomPosInRadius(), 1f);
        }

        int materialRange = Random.Range(materialMinMax.x, materialMinMax.y + 1);
        for (int i = 0; i < materialRange; i++)
        {
            GameManager.current.eventService.SpawnMaterial(GetRandomPosInRadius(), 1f);
        }

        int upgradeRange = Random.Range(upgradeMinMax.x, upgradeMinMax.y + 1);
        for (int i = 0; i < upgradeRange; i++)
        {
            GameManager.current.eventService.SpawnRandomUpgrade(GetRandomPosInRadius());
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(transform.localScale.x + lootSpreadRange,
                                                            0.25f,
                                                            transform.localScale.z + lootSpreadRange));
    }
}
