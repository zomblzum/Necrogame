using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPriorityZone : MonoBehaviour
{
    [Header("Дистанция включения преследования игрока")]
    public float aggroDistance = 50f;
    [Header("Триггер зона для приоритетных миньнонов")]
    public CapsuleCollider aggroAreaCollider;
    [Header("Враг с приоритетной зоной")]
    public Enemy enemy;

    private List<GameObject> priorityTargets;

    void Awake()
    {
        priorityTargets = new List<GameObject>();
        aggroAreaCollider.radius = aggroDistance;
    }

    // Срабатывает при входе игрока и миньонов в область видимости врага
    protected virtual void OnTriggerEnter(Collider other)
    {
        return;
        if (other.gameObject.GetComponent<Minion>() || other.gameObject.GetComponent<Player>())
        {
            priorityTargets.Add(other.gameObject);

            if(!priorityTargets.Contains(enemy.GetCurrentTarget()))
            {
                enemy.SetAttackTarget(other.gameObject);
            }
        }
    }

    // Срабатывает, когда игрок или его прислужник умирают/уходят из зоны видимости
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Minion>() || other.gameObject.GetComponent<Player>() || enemy.GetCurrentTarget() == null)
        {
            priorityTargets.Remove(other.gameObject);
            priorityTargets.RemoveAll(item => item == null);

            if (enemy.GetCurrentTarget() == other.gameObject || enemy.GetCurrentTarget() == null)
            {
                enemy.GetClosestTarget();
            }
        }
    }
}
