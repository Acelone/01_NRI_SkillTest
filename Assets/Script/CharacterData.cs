using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Character",menuName ="Character")]
public class CharacterData : ScriptableObject
{
    public int baseHealth;
    public int baseAttack;
    public int baseDefense;
    public CharacterType characterType;

    public enum CharacterType
    {
        empty,
        Player,
        Enemy
    }
}
