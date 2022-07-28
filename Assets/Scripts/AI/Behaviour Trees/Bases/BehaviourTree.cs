using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourTree
{
    protected Agent ownerAgent { get; private set; }
    public Node rootNode { get; protected set; }

    public BehaviourTree(Agent owner)
    {
        ownerAgent = owner;
    }

    public void Update()
    {
        rootNode.Update();
    } 
}
