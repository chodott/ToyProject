using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData
{
    public int Id;
    public int CharacterNumber;

    public PlayerData(int id, int characterNumber) 
    {
        this.Id = id;
        this.CharacterNumber = characterNumber;
    }
}
