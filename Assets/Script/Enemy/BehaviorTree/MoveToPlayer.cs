using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : Node
{
    private float _range = 3.0f;
    private Transform _transform;
    private Transform _targetTransform;
    
    public MoveToPlayer(Transform transform,  Transform targetTransform)
    {
        _transform = transform;
        _targetTransform = targetTransform;
    }
    public override NodeState Evaluate()
    {
        Vector2 position = new(_transform.position.x, _transform.position.y);
        Vector2 targetPosition = new(_targetTransform.position.x, _targetTransform.position.y);
        float distance = (position - targetPosition).magnitude;
        PlayerController controller = _transform.GetComponent<PlayerController>();

        if (distance <= _range)
        {
            controller.Stop();
            return NodeState.Success;
        }
        else
        {
            Vector2 directionVec = targetPosition - position;
            controller.Run(directionVec);
            controller.Turn(directionVec);
            return NodeState.Running;
        }
    }
}
