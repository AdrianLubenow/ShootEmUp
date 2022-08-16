using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/BulletAmountBuff")]
public class BuffBulletAmount : PowerupEffect
{
    public override void Apply(GameObject target)
    {
        //target.GetComponent<PlayerController>().activeWeapons.Add();

        target.GetComponent<PlayerController>().AddBulletPoint();
    }

}
