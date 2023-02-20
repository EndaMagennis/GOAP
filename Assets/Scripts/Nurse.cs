using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Nurse : GAgent
{
    protected override void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("treatPatient", 1, false);
        goals.Add(s1, 3);

        SubGoal s2 = new SubGoal("rested", 1, false);
        goals.Add(s2, 1);

        Invoke("GetTired", Random.Range(20, 30));
    }

    void GetTired()
    {
        beliefs.ModifyState("exhausted", 0);
        Invoke("GetTired", Random.Range(20, 30));
    }
}
