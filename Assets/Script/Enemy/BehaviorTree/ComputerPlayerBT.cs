using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayerBT : BehaviorTree
{
    private Transform _targetTransform;
    public Transform Target
    {
        set { _targetTransform = value; }
    }

    protected override Node SetupBT()
    {
        Node root = new SelectorNode(new List<Node>
        {
            new SequenceNode(new List<Node>
            {
                new CheckAbleAttack(transform, _targetTransform),

            }),

            new MoveToPlayer(transform, _targetTransform)
        }) ;

        transform.GetComponent<PlayerController>().ChangeForm(5);

        return root;
    }

}
