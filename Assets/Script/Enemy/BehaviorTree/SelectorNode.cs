using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : Node
{
    public SelectorNode(List<Node> children) : base(children) { }
    public override NodeState Evaluate()
    {
        foreach(Node node in _childrenList)
        {
            switch(node.Evaluate())
            {
                case NodeState.Running:
                    return _curState = NodeState.Running;
                case NodeState.Success:
                    return _curState = NodeState.Success;
                case NodeState.Failure:
                    continue;
            }
        }
        return _curState = NodeState.Failure;
    }
}