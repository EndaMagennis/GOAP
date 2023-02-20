using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Creating a dictionary of Key-Value pairs to hold state info for various objects in
 game environment. Will be accessed by other classes in order for the agent to make decisions
about their behaviour*/
[System.Serializable]
public class WorldState 
{
    //for creating a dictionary
    public string key;
    public int value;
}
public class WorldStates
{
    public Dictionary<string, int> states;

    public WorldStates()
    {
        states = new Dictionary<string, int>();
    }

    public bool HasState(string key)
    {
        return states.ContainsKey(key);
    }

    void AddState(string key, int value)
    {
        states.Add(key, value);
    }

    public void ModifyState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] += value;
            if (states[key] <= 0)
            {
                RemoveState(key);
            }
        }
        else
        {
            states.Add(key, value);
        }
    }
    public void RemoveState(string key)
    {
        if(states.ContainsKey(key))
            states.Remove(key);
    }

    public void SetState(string key, int value)
    {
        if(states.ContainsKey(key))
            states[key] = value;
        else
            states.Add(key, value);
    }

    public Dictionary<string, int> GetStates()
    {
        return states;
    }
}
