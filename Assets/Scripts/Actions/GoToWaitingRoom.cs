using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWaitingRoom : GAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("Waiting", 1);// adds total number of agents waiting to world
        GWorld.Instance.AddPatient(this.gameObject);
        beliefs.ModifyState("atHospital", 1);// adds a belief state to individual agents
        return true;
    }
        
}
