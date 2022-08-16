using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/SpecialChargeBuff")]
public class BuffSpecialCharge : PowerupEffect
{
    public float amountToBuff;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerController>().UpdateSpecialChargeOnPowerup(amountToBuff);
    }
}
