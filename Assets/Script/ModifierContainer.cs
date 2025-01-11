using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierContainer : MonoBehaviour
{
    public static ModifierContainer instance;
    public StatsModifier defenseBuff;
    public StatsModifier ultimateAttack;
    public StatsModifier attackBuff;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(this.gameObject);
        }
    }
}
