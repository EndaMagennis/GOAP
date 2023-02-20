using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPatient : GAction
{
    GameObject resource;

    public override bool PrePerform()
    {
        target = GWorld.Instance.RemovePatient();//setting target as patient to remove from waitingroom
        if(target == null)
            return false;

        resource = GWorld.Instance.RemoveCubicle();//setting resource as cubicle to remove from freeCubicle list
        if (resource != null)// if there is a free cubicle
            inventory.AddItem(resource);//adding it to nurse inventory
        else
        {
            GWorld.Instance.AddPatient(target);//if no freecubicle, adding patient to waiting list again
            target = null;// removing the nurse's target
            return false;
        }
        GWorld.Instance.GetWorld().ModifyState("FreeCubicle", -1);//choosing a cubicle and removing it from freeCubicle
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("Waiting", -1);//removing an agent from the waiting state
        if (target)
            target.GetComponent<GAgent>().inventory.AddItem(resource);//adding cubicle to patient inventory, so both have same cubicle

        return true;
    }
}
