using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAbleAttack : Node
{
    private Transform _transform;
    private PlayerController _targetPlayer;

    public override NodeState Evaluate()
    {
        RaycastHit hit;
        Vector3 directionVec = _targetPlayer.transform.position - _transform.position;
        if(Physics.Raycast(_transform.position, directionVec, out hit))
        {
            if (hit.transform.GetComponent<PlayerController>() == null) 
                return NodeState.Failure;
            else 
                return NodeState.Success;

            //ÃÑ¾Ë ÄðÅ¸ÀÓµµ ºÁ¾ß´ë
        }
        return NodeState.Failure;
    }
}
