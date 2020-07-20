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
        attackTargets = new List<GameObject>();
        aggroAreaCollider.radius = aggroDistance;
    }

    // Срабатывает при входе игрока и миньонов в область видимости врага
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Minion>() != null || other.gameObject.GetComponent<Player>() != null)
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
        attackTargets.RemoveAll(item => item == null);
        if (other.gameObject.GetComponent<Minion>() != null || other.gameObject.GetComponent<Player>() != null)
        {
            attackTargets.Remove(other.gameObject);

            if (attackTarget == other.gameObject)
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
