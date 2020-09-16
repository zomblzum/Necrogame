using UnityEngine;
using UnityEngine.AI;

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

    protected override void Start()
    {
        base.Start();
    }

    protected override bool IgnoreWrongPath()
    {
        if (attackTarget && attackTarget.GetComponent<Player>())
            return true;
        else
            return base.IgnoreWrongPath();
    }
}