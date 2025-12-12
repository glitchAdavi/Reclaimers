using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_building : Ability
{
    public override void Effect()
    {
        if (Physics.Raycast(GameManager.current.playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out RaycastHit info, 100f, 1 << 20))
        {
            if (GameManager.current.tileService.IsPositionInsidePlayableArea(info.point))
            {
                Instantiate(aPrefab, new Vector3(info.point.x, 0.5f, info.point.z), Quaternion.identity)
                    .GetComponent<Mine>().Init(aDamage, aRadius, 0f, aModifier);
            }
            
        }
    }
}
