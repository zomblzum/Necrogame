using UnityEngine;

/// <summary>
/// Реализация простого врага
/// </summary>
public class Enemy : Character
{
    public override void GetHit(GameObject attackFrom, int damage)
    {
        base.GetHit(attackFrom, damage);

        if (attackTarget == null)
        {
            attackTarget = attackFrom;
        }
	}
}