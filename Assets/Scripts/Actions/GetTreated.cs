using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTreated : GAction
{
    public override bool PrePerform()
    {
        target = inventory.FindItemWithTag("Cubicle");// getting the cubicle from inventory
        if(target == null)
            return false;
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("Treated", 1);// adding 'Treated' as a state to the worldStates
        beliefs.ModifyState("isCured", 1);
        inventory.RemoveItem(target);//removing cubicle after treatment
        return true;
    }
}
