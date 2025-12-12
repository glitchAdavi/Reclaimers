using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_grenade : Ability
{

    public override void Effect()
    {
        if (Physics.Raycast(GameManager.current.playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out RaycastHit info, 100f, 1 << 20))
        {
            Instantiate(GameManager.current.gameInfo.throwablePrefab, new Vector3(info.point.x, 0.1f, info.point.z), Quaternion.identity)
                .GetComponent<Throwable>().Init(transform.position, info.point, 1f, statBlock.abilityThrowableSprite);
            Instantiate(aPrefab, new Vector3(info.point.x, 0.5f, info.point.z), Quaternion.identity)
                .GetComponent<Explosive>().Init(aDamage, aRadius, 1f, aModifier);
        }
    }
}
