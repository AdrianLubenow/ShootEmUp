using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Powerups/SpeedBuff")]
public class BuffSpeed : PowerupEffect
{
    public float amountToBuff;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerController>().AddMoveSpeed(amountToBuff);
    }
}
