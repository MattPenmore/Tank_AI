using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Composite : Node
{
    // Start is called before the first frame update
    public List<Node> childNodes { get; private set; }
    protected int currentIndex = 0;

    protected Composite(Agent ownerAgent) : base(ownerAgent)
    {
        currentIndex = 0; childNodes = new List<Node>();
    }

    public void AddChild(Node newChild)
    {
        childNodes.Add(newChild);
    }

    protected void Reset()
    {
        currentIndex = 0;
    }
}
