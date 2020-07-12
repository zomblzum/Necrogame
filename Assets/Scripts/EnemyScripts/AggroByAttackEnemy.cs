using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Враг, переагривающийся на последнюю атаковавшую цель, если основная цель не бьет
/// </summary>
public class AggroByAttackEnemy : AutoAggroEnemy
{
    [Header("Время, после которого сменится цель, если текущая цель не будет атаковать")]
    public float waitTime = 3f;

    private GameObject lastAttackedTarget;
    private float curTime = 0f;

    public override void GetHit(GameObject attackFrom, int damage)
    {
        base.GetHit(attackFrom, damage);

        lastAttackedTarget = attackFrom;

        if(attackFrom == GetCurrentTarget())
        {
            curTime = 0f;
        }
    }

    private void Update()
    {
        if(curTime >= waitTime && lastAttackedTarget != null)
        {
            SetAttackTarget(lastAttackedTarget);
        }
        else
        {
            curTime += Time.deltaTime;
        }
    }
}