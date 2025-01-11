using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "StatsModifier/NewModifier")]
public class StatsModifier : ScriptableObject
{
    public string modifierName;
    public string describtion;
    public int duration;
    public float attackEffect;
    public float defenseEffect;
    public int DefenseUpBuff(int baseDefense)
    {
        return Mathf.CeilToInt(baseDefense * defenseEffect);
    }
    public int AttackUp(int baseAttack)
    {
        return Mathf.CeilToInt(baseAttack * attackEffect);
    }
}
