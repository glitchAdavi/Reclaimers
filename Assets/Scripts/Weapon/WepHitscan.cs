using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WepHitscan : Weapon
{
    private Ray myRay;

    protected override void ShootEffect()
    {
        Ray ray = GameManager.current.playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit info, 100f, 1 << 20))
        {
            Debug.DrawLine(transform.position, info.point, Color.red, 0.5f);
            
            
            
            
            //myRay = new Ray()
        }
    }
}
