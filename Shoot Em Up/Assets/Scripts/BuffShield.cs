using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/ShieldBuff")]
public class BuffShield : PowerupEffect
{
    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerController>().ActivateShield();
    }
}
