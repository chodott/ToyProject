using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateContext : MonoBehaviour
{
    // Start is called before the first frame update

    public IPlayableState CurrentState{ get; set; }

    private readonly PlayerController _playerController;

    public StateContext(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void Transition()
    {
        CurrentState.Handle(_playerController); 
    }
    public void Transition(IPlayableState state)
    {
        
        CurrentState = state;
        CurrentState.Handle(_playerController);
    }

    public void Update()
    {
        CurrentState.Update();
    }
}
