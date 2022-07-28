using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1.0f;
    public GameObject target;
    public Vector3 targetLocation;
    public string targetTag;
    public float duration = 0;
    public float maxTime = 3;
    public float startTime;
    public WorldState[] preConditions;
    public WorldState[] afterEffects;
    public NavMeshAgent agent;

    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;

    public WorldStates agentBeliefs;

    public bool running = false;

    public GAction()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        //Get nav mesh agent
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        //Get preconditions for action
        if(preConditions != null)
        {
            foreach(WorldState w in preConditions)
            {
                preconditions.Add(w.key, w.value);
            }
        }
        //Get after effects for action
        if (afterEffects != null)
        {
            foreach (WorldState w in afterEffects)
            {
                effects.Add(w.key, w.value);
            }
        }
    }

    public bool IsAchievable()
    {
        return true;
    }

    //Check if conditions contains required preconditions
    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        int i = 0;
        foreach(KeyValuePair<string, int> p in preconditions)
        {
            //Check if contains key
            if (conditions.ContainsKey(p.Key))
            {
                //Check if key has correct value
                if(conditions[p.Key] != p.Value)
                {
                    //If any of preconditions wrong, action not achievable
                    return false;
                }
            }
            i++;
        }
        return true;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
