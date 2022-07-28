using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class SubGoal
{
    public Dictionary<string, int> sgoals;
    public bool remove;

    public SubGoal(string s, int i, bool r)
    {
        sgoals = new Dictionary<string, int>();
        sgoals.Add(s, i);
        remove = r;
    }
}


public class GAgent : MonoBehaviour
{
    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    public GPlanner planner;
    public Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currentGoal;
    float accepetanceDistance = 5f;


    // Start is called before the first frame update
    public void Start()
    {
        //Get all actions on the tank
        GAction[] acts = this.GetComponents<GAction>();
        foreach(GAction a in acts)
        {
            actions.Add(a);
        }
    }

    bool invoked = false;
    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    void LateUpdate()
    {
        

        //If an action is currently happening
        if (currentAction != null && currentAction.running)
        {
            //If can't find path, find nearest location to destination, it can make path to
            NavMeshPath path = new NavMeshPath();
            NavMeshHit hit;
            currentAction.agent.CalculatePath(currentAction.target.transform.position, path);
            if (path.status == NavMeshPathStatus.PathPartial)
            {
                if (NavMesh.SamplePosition(currentAction.target.transform.position, out hit, 3.0f, NavMesh.AllAreas))
                {
                    currentAction.target.transform.position = hit.position;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
            }
            //If destination is reached, or enough time has passed, complete the action
            if (currentAction.agent.hasPath && (currentAction.agent.remainingDistance < accepetanceDistance || Time.time - currentAction.maxTime > currentAction.startTime))
            {
                if(!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }

            //Complete action if doesn't have path and gone on too long
            if(!currentAction.agent.hasPath && Time.time - currentAction.maxTime > currentAction.startTime)
            {
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            //Don't continue with rest of update if action is currently happening
            return;
        }

        //If there is no planner, or plan has no actions, create a new plan
        if(planner == null || actionQueue == null)
        {
            planner = new GPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach(KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.plan(actions, sg.Key.sgoals, null);
                if(actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        //If all actions in queue finished, remove plan
        if(actionQueue != null && actionQueue.Count == 0)
        {
            //Remove goal if it only needs to be done once
            if(currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            planner = null;
        }

        //If have actions in queue
        if(actionQueue != null && actionQueue.Count > 0)
        {
            //Set current action and take off of queue
            currentAction = actionQueue.Dequeue();
            //If can be performed, set destination, else remove queue
            if(currentAction.PrePerform())
            {
                if (currentAction.target == null && currentAction.targetTag != "")
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);

                if(currentAction.target != null)
                {
                    currentAction.running = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
            }
            else
            {
                actionQueue = null;
            }
        }
    }
}
