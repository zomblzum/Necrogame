
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Бес
/// Выходит из под контроля и атакует игрока спустя промежуток времени
/// </summary>
public class DemonMinion : Minion
{
    [Header("Время стана, который получает враг от демона")]
    public float stunTimeFromAttack = 1f;

    private float normalAngularSpeed;
    private bool startAttack;

    protected override void Awake()
    {
        base.Awake();
        //Отключаем обход препятствий
        //agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
        normalAngularSpeed = agent.angularSpeed;
        startAttack = false;
    }

    protected override void CloseTargetInteraction()
    {
        agent.angularSpeed = 0f;
        agent.SetDestination(transform.forward + new Vector3(10,10));
        
        if(!startAttack)
        {
            startAttack = true;
            StartCoroutine(ReturnToNormalMoving());
        }
    }

    private IEnumerator ReturnToNormalMoving()
    {
        yield return 4f;
        agent.SetDestination(attackTarget.transform.position);
        agent.angularSpeed = normalAngularSpeed;
        startAttack = false;
    }

    private void OnTriggerStay(Collider other)
    {
        //agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;

        if(underControl)
        {
            // Если под контролем, то станим только врагов
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.StunForTime(stunTimeFromAttack);
            }
        }
        else
        {
            // Если нет, то всех кого можем
            IStunable stunableCharacter = other.gameObject.GetComponent<IStunable>();

            if (stunableCharacter != null)
            {
                stunableCharacter.StunForTime(stunTimeFromAttack);
            }
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
