using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/BulletDamageBuff")]
public class BuffBulletDamage : PowerupEffect
{
    public float amountToBuff;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerController>().IncreaseBulletDamage(amountToBuff);
    }
}
