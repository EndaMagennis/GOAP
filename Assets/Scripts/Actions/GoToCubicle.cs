using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToCubicle : GAction
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
        GWorld.Instance.GetWorld().ModifyState("TreatingPatient", 1);// adding 'Treated' as a state to the worldStates
        GWorld.Instance.AddCubicle(target);
        inventory.RemoveItem(target);//removing cubicle after treatment
        GWorld.Instance.GetWorld().ModifyState("FreeCubicle", 1);// nurse makes a free cubicle again

        return true;
    }
}
