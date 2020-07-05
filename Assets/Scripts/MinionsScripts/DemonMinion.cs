
using System;
using UnityEngine;

/// <summary>
/// Бес
/// Выходит из под контроля и атакует игрока спустя промежуток времени
/// </summary>
public class DemonMinion : Minion
{
    [Header("Время стана, который получает враг от демона")]
    public float stunTimeFromAttack = 1f;

    public override void Hit()
    {
        base.Hit();

        if (attackTarget != null && attackTarget.GetComponent<IStunable>() != null)
        {
            attackTarget.GetComponent<IStunable>().StunForTime(stunTimeFromAttack);
        }
    }

    public override void GoOutControl()
    {
        underControl = false;
        targetsTags.Add("Minion");
        targetsTags.Add("Player");
        FindTargetsByTags();
        GetClosestTarget();
    }

    public override void GoUnderControl()
    {
        underControl = true;
        targetsTags.Remove("Minion");
        targetsTags.Remove("Player");
        FindTargetsByTags();
        GetClosestTarget();
    }
}
