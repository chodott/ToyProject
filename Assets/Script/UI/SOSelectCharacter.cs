using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character/ScriptableObject", order = 1)]
public class SOSelectCharacter : ScriptableObject
{
    [SerializeField]
    public Sprite Thumbnail;
    [SerializeField]
    [Range(0, 20)]
    public int Number;
}
