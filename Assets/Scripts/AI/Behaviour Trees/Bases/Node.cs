using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NODE_STATUS
{
    SUCCESS,
    FAILURE,
    RUNNING,
    NONE
}



public abstract class Node
{
    public Agent ownerAgent { get; private set; }

    public Node(Agent ownerAgent)
    {
        this.ownerAgent = ownerAgent;
    }

    public virtual NODE_STATUS Update()
    {
        return NODE_STATUS.NONE;
    }
}
