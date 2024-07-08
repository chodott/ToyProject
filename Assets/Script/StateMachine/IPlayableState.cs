using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayableState
{
    void Handle(PlayerController playerController);
    void Update();
}