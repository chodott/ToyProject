using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : Node
{
    private float _range = 3.0f;
    private Transform _transform;
    private PlayerController _targetPlayer;
    public override NodeState Evaluate()
    {
        return NodeState.Success;
    }
}
