using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    Running,
    Failure,
    Success
}

public abstract class Node
{
    protected NodeState _curState;
    public Node ParentNode;
    protected List<Node> _childrenList = new();

    public Node()
    {
        ParentNode = null;
    }

    public Node(List<Node> children)
    {
        foreach(Node child in children) 
        {
            _childrenList.Add(child);
            child.ParentNode = this;
        }
    }

    public abstract NodeState Evaluate();
}