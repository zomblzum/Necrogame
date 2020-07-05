using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Враг, автоматически начинающий бой, после получения урона
/// </summary>
public class AggroByAttackEnemy : Enemy
{
    public override void GetHit(GameObject attackFrom, int damage)
    {
        base.GetHit(attackFrom, damage);
        inAggro = true;
    }

    public override void StunForTime(float stunTime)
    {
        base.StunForTime(stunTime);
        inAggro = true;
    }
}