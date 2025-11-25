using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IO_Container : InteractableObject
{
    public float lootSpreadRange = 3f;

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


    protected override void OnFinishEffect()
    {
        Debug.Log("opened");
    }
}
