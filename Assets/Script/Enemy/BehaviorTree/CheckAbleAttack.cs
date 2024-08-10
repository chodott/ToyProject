using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAbleAttack : Node
{
    private Transform _transform;
    private Transform _targetTransform;

    public CheckAbleAttack(Transform transform, Transform targetTransform)
    {
        _transform = transform;
        _targetTransform = targetTransform;
    }

    public override NodeState Evaluate()
    {
        RaycastHit hit;
        Vector3 directionVec = _targetTransform.position - _transform.position;
        if(Physics.Raycast(_transform.position, directionVec, out hit))
        {
            PlayerController targetPlayer = hit.transform.GetComponent<PlayerController>();
            if (targetPlayer == null) 
                return NodeState.Failure;
            else
            {
                PlayerController controller = _transform.GetComponent<PlayerController>();
                controller.Turn(directionVec);
                controller.Fire();
                return NodeState.Success;
            }

        }
        return NodeState.Failure;
    }
}
