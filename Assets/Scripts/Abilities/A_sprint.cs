using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class A_sprint : Ability
{
    public override void Effect()
    {
        owner.AddModifier(GameManager.current.GetModifier("speed"));
    }
}
