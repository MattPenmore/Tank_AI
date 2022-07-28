using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NodePlanner
{
    public NodePlanner parent;
    public float cost;
    public Dictionary<string, int> state;
    public GAction action;

    public NodePlanner(NodePlanner parent, float cost, Dictionary<string, int> allstates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allstates);
        this.action = action;
    }
}


public class GPlanner
{
    public Queue<GAction> plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates states)
    {
        //Create list of achievable actions
        List<GAction> usableActions = new List<GAction>();
        foreach(GAction a in actions)
        {
            if(a.IsAchievable())
            {
                usableActions.Add(a);
            }
        }

        //Nodes which have reached goal
        List<NodePlanner> leaves = new List<NodePlanner>();

        //Create starting node on tree, with states equal to world states
        NodePlanner start = new NodePlanner(null, 0, GWorld.Instance.GetWorld().GetStates(), null);

        //Create graph of paths towards goal
        bool success = BuildGraph(start, leaves, usableActions, goal);

        //If unable to build graph return with no plan
        if(!success)
        {
            Debug.Log("NO PLAN");
            return null;
        }

        //Find cheapest node
        NodePlanner cheapest = null;
        foreach(NodePlanner leaf in leaves)
        {
            if (cheapest == null)
                cheapest = leaf;
            else
            {
                if (leaf.cost < cheapest.cost)
                    cheapest = leaf;
            }
        }

        //Go back up graph using node parents, to find all actions in cheapest path
        List<GAction> result = new List<GAction>();
        NodePlanner n = cheapest;
        while(n != null)
        {
            if(n.action != null)
            {
                result.Insert(0, n.action);
            }

            n = n.parent;
        }

        //Add all nodes in cheapest path to queue
        Queue<GAction> queue = new Queue<GAction>();
        foreach (GAction a in result)
        {
            queue.Enqueue(a);
        }

        Debug.Log("The Plan is: ");
        foreach(GAction a in queue)
        {
            Debug.Log("Q: " + a.actionName);
        }

        return queue;
    }

    private bool BuildGraph(NodePlanner parent, List<NodePlanner> leaves, List<GAction> useableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach(GAction action in useableActions)
        {
            //If action is achievable, with world state after parent
            if(action.IsAchievableGiven(parent.state))
            {
                //Set state to parents state
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                //Add states in this actions effects
                foreach(KeyValuePair<string, int> eff in action.effects)
                {
                    if (!currentState.ContainsKey(eff.Key))
                        currentState.Add(eff.Key, eff.Value);
                }

                // make a new node, using new, state and cost
                NodePlanner node = new NodePlanner(parent, parent.cost + action.cost, currentState, action);

                //If reached goal, add to leaves
                if(GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    //Repeat progress using new node as starting point
                    List<GAction> subset = ActionSubset(useableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                        foundPath = true;
                }
            }
        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach(KeyValuePair<string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key))
                return false;
        }
        return true;
    }

    private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
    {
        List<GAction> subset = new List<GAction>();
        foreach(GAction a in actions)
        {
            if (!a.Equals(removeMe))
                subset.Add(a);
        }
        return subset;
    }
}
