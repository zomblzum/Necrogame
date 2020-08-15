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
    [Header("Время переключения на случайную цель при выъоде из контроля")]
    public float changeRandomTargetTime = 30f;

    private float normalAngularSpeed;
    private float curTime;
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
        if(!startAttack)
        {
            startAttack = true;
            StartCoroutine(ReturnToNormalMoving());
        }
    }

    protected override void RunToEnemy()
    {
        if(!startAttack)
        {
            base.RunToEnemy();
        }
    }

    IEnumerator ReturnToNormalMoving()
    {
        //это работает, хз почему, лучше не трогать никогда
        agent.angularSpeed = 0f;
        agent.SetDestination(transform.forward * 5);

        yield return new WaitForSeconds(1f);

        agent.SetDestination(attackTarget.transform.position);
        agent.angularSpeed = normalAngularSpeed;
        startAttack = false;
    }

    protected override void Update()
    {
        base.Update();
        if (!underControl)
        {
            if(curTime >= changeRandomTargetTime)
            {
                GetRandomTarget();
            }
            else
            {
                curTime += Time.deltaTime;
            }
        }
    }

    protected override void CharacterBehaviour()
    {
        if(underControl)
        {
            base.CharacterBehaviour();
        }
        else
        {
            if(!inAggro || attackTarget == null)
            {
                FindTargetsByTags();
                GetClosestTarget();
                inAggro = true;
            }
            AttackCommand();
        }
    }

    public override void FindTargetsByTags()
    {
        base.FindTargetsByTags();
        // Демоны не атакуют других демонов
        attackTargets.RemoveAll(item => item.GetComponent<DemonMinion>() != null);
    }

    private void OnTriggerEnter(Collider other)
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

            if (stunableCharacter != null && other.gameObject.GetComponent<DemonMinion>() == null)
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
        curTime = 0;
        FindTargetsByTags();
        GetRandomTarget();
    }

    public override void GoUnderControl()
    {
        underControl = true;
        targetsTags.Remove("Minion");
        targetsTags.Remove("Player");
        FindTargetsByTags();
        GetClosestTarget();
    }

    private void GetRandomTarget()
    {
        attackTarget = attackTargets[Random.Range(0, attackTargets.Count)];
    }
}
