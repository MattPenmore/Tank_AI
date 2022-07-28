using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WorldState
{
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

    //Check if worldstate has a key
    public bool HasState(string key)
    {
        return states.ContainsKey(key);
    }

    // Add a new state to worldstate
    void AddState(string key, int value)
    {
        states.Add(key, value);
    }

    //If a key exists, add to it's value, otherwise create state with given value
    public void ModifyState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] += value;
            if (states[key] <= 0)
                RemoveState(key);
        }
        else
            states.Add(key, value);
    }

    //Remove a world state
    public void RemoveState(string key)
    {
        if (states.ContainsKey(key))
            states.Remove(key);
    }

    //Set the value of a state. If the state doesn't exist add it.
    public void SetState(string key, int value)
    {
        if (states.ContainsKey(key))
            states[key] = value;
        else
            states.Add(key, value);

        Debug.Log(key + " , " + value.ToString());
    }

    //Return states
    public Dictionary<string, int> GetStates()
    {
        return states;
    }
}
