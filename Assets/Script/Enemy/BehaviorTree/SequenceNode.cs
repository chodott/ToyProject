using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : Node
{
    public SequenceNode(List<Node> children) : base(children) { }
    public override NodeState Evaluate()
    {
        bool isRunning = false;
        foreach(Node node in _childrenList)
        {
            switch(node.Evaluate())
            {
                case NodeState.Running:
                    isRunning = true;
                    break;
                case NodeState.Success:
                    break;
                case NodeState.Failure:
                    return _curState = NodeState.Failure;
            }
        }

        return _curState = isRunning? NodeState.Running : NodeState.Success;
    }
}