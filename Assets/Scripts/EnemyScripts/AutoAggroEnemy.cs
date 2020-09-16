using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoAggroEnemy : Enemy
{
    [Header("Дистанция включения преследования игрока")]
	public float aggroDistance = 50f;
	[Header("Триггер зона для обнаружения миньнонов")]
	public CapsuleCollider aggroAreaCollider;

    void Awake()
    {
        aggroAreaCollider.radius = aggroDistance;
    }

    // Срабатывает при входе игрока и миньонов в область видимости врага
    protected virtual void OnTriggerEnter(Collider other)
    {
        //if(attackTargets.Contains(other.gameObject))
        //{
        //    return;
        //}

        if (other.gameObject.GetComponent<Minion>() || other.gameObject.GetComponent<Player>())
        {
            attackTargets.Add(other.gameObject);
            inAggro = true;

            if(attackTarget == null)
            {
                attackTarget = other.gameObject;
            }
        }
    }

    // Срабатывает, когда игрок или его прислужник умирают/уходят из зоны видимости
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Minion>() || other.gameObject.GetComponent<Player>() || attackTarget == null)
        {
            attackTargets.RemoveAll(item => item == null);
            attackTargets.Remove(other.gameObject);

            if (attackTarget == other.gameObject || attackTarget == null)
            {
                GetClosestTarget();
            }
        }

        if (attackTargets.Count == 0)
        {
            inAggro = false;
        }
    }
}
