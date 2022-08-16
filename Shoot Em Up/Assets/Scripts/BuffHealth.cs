using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Powerups/HealthBuff")]
public class BuffHealth : PowerupEffect
{
    public float amountToHeal;
    
    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerHealth>().AddHealth(amountToHeal);
    }
}
