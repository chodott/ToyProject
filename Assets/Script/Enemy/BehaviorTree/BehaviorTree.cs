using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    private Node _rootNode;
    private void Start()
    {
        _rootNode = SetupBT();
    }

    private void Update()
    {
        if (_rootNode != null) return;
        _rootNode.Evaluate();
    }

    protected abstract Node SetupBT();
}
