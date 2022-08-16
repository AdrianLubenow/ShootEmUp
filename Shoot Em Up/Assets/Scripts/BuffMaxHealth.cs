using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/MaxHealthBuff")]
public class BuffMaxHealth : PowerupEffect
{
    private float amountToBuff = 1000f;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerHealth>().AddMaxHealthOnPowerup(amountToBuff);
    }
}