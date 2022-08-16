using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Powerups/BulletSpeedBuff")]
public class BuffBulletSpeed : PowerupEffect
{
    public float amountToBuff;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerController>().IncreaseBulletSpeed(amountToBuff);
    }
}
