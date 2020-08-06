﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Character : MonoBehaviour, IAttackable, IDieable, IStunable
{
    [Header("Имя персонажа")]  
    public string characterName;
    [Header("Максимальное здоровье")] 
    public int health = 100;
    [Header("Труп")] 
    public Corpse corpse;
    [Header("VFX при смерти")] 
    public GameObject deathEffect;
    [Header("VFX при стане")] 
    public ParticleSystem stunEffect;
    [Header("Атакует цель")] 
    public bool inAggro = true;
    [Header("Сила атаки")] 
    public int attack = 1;
    [Header("Скорость атаки")] 
    public float attackSpeed = 1f;
    [Header("Дальность атаки")] 
    public float attackDistance = 1f;
    [Header("NavMeshAgent")]
    public NavMeshAgent agent;
    [Header("Аниматор")]
    public Animator animator;

    protected CharacterUI characterUI;
    protected GameObject attackTarget;
    protected List<GameObject> attackTargets;
    protected float stunDuration;
    protected int attackSpeedFloat;
    protected int attackBool;
    protected int speedFloat;
    protected int curHealth;
    protected int hitBool;
    protected int stunBool;
    protected Vector3 moveTarget;


    /// <summary>
    /// Инициализация персонажа
    /// </summary>
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterUI = GetComponent<CharacterUI>();

        hitBool = Animator.StringToHash("GetHit");
        stunBool = Animator.StringToHash("Stun");
        attackSpeedFloat = Animator.StringToHash("AttackSpeed");
        attackBool = Animator.StringToHash("Attack");
        speedFloat = Animator.StringToHash("Speed");
        animator.SetBool(attackBool, false);

        characterUI.healthBar.maxValue = health;
        characterUI.UpdateHealthBar(health);
        characterUI.SetName(characterName);

        stunEffect.Stop();
        stunDuration = 0f;
        curHealth = health;

        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }

    /// <summary>
    /// Назначить приоритетную цель
    /// </summary>
    public void SetAttackTarget(GameObject attackable)
    {
        if(attackable != null)
        {
            attackTarget = attackable;
            inAggro = true;
        }
    }

    /// <summary>
    /// Текущая цель для атаки
    /// </summary>
    public GameObject GetCurrentTarget()
    {
        return attackTarget;
    }

    public virtual void SetMoveTarget(Vector3 moveTarget)
    {
        this.moveTarget = moveTarget;
    }

    private void FixedUpdate()
    {
        if (stunDuration <= 0)
        {
            animator.SetBool(stunBool, false);
            stunEffect.Stop();
            CharacterBehaviour();
        }
        else
        {
            animator.SetBool(stunBool, true);
            PassiveBehaviour();
            if (!stunEffect.isPlaying)
            {
                stunEffect.Play();
            }
            stunDuration -= Time.deltaTime;
        }
    }

    public virtual void GetHit(GameObject attackFrom, int damage)
    {
        if (curHealth > 0)
        {
            animator.SetBool(hitBool,true);
            curHealth -= damage;
            characterUI.UpdateHealthBar(curHealth);
            if (curHealth <= 0)
            {
                curHealth = 0;
                Die();
            }
            StartCoroutine(StopHitAnimation());
        }
    }

    /// <summary>
    /// Атакуем
    /// </summary>
    public virtual void Attack()
    {
        animator.SetBool(attackBool, true);
        animator.SetFloat(attackSpeedFloat, attackSpeed);
    }

    private IEnumerator StopHitAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool(hitBool, false);
    }

    /// <summary>
    /// Постоянное поведение персонажа
    /// </summary>
    protected virtual void CharacterBehaviour()
    {
        if(moveTarget != Vector3.zero)
        {
            MovingBehaviour();
        }

        if (!inAggro)
        {
            PassiveBehaviour();
        } 
        else if (attackTarget != null)
        {
            AttackTargetInteractionsBehaviour();
        }
    }

    /// <summary>
    /// Поведение персонажа, когда нет цели или в стане
    /// </summary>
    protected virtual void PassiveBehaviour()
    {
        SetMoveTarget(Vector3.zero);
        agent.SetDestination(Vector3.zero);
        agent.isStopped = true;
        animator.SetFloat(speedFloat, 0f);
    }

    /// <summary>
    /// Поведение при беге
    /// </summary>
    protected virtual void MovingBehaviour()
    {
        //Вынужденный костыль с Y
        moveTarget.y = transform.position.y;

        agent.SetDestination(moveTarget);

        //костыль для нормального поворота нпс
        transform.LookAt(agent.steeringTarget);


        if (Vector3.Distance(transform.position, moveTarget) <= agent.radius * 2 && attackTarget == null)
        {
            // очередной костыль, без которого миньон разворачивается на рандомный угол при остановке
            transform.LookAt(moveTarget);
            PassiveBehaviour();
        } 
        else if (animator.GetFloat(speedFloat) == 0)
        {
            agent.isStopped = false;
            animator.SetFloat(speedFloat, 1f);
        }
    }

    /// <summary>
    /// Передвижение к цели и ее атака
    /// </summary>
    protected virtual void AttackTargetInteractionsBehaviour()
    {
        if (Vector3.Distance(transform.position, attackTarget.transform.position) <= attackDistance && stunDuration <= 0 && inAggro)
        {
            CloseTargetInteraction();
        }
        else
        {
            RunToEnemy();
        }
    }

    /// <summary>
    /// Взаимодействие с целю, когда есть возможность ее атаковать
    /// </summary>
    protected virtual void CloseTargetInteraction()
    {
        transform.LookAt(attackTarget.transform);
        PassiveBehaviour();
        if (!animator.GetBool(attackBool))
        {
            animator.SetFloat(speedFloat, 0f);
            Attack();
        }
    }

    /// <summary>
    /// Преследовать цель
    /// </summary>
    protected virtual void RunToEnemy()
    {
        SetMoveTarget(attackTarget.transform.position);
    }

    /// <summary>
    /// Смерть
    /// </summary>
    public virtual void Die()
    {
        Vector3 corpsePost = transform.position;
        // Сначала отключаем коллайдер на этом персонаже, иначе труп может улететь из за колизии
        gameObject.GetComponent<Collider>().enabled = false;
        // Спамим труп
        Instantiate(deathEffect, new Vector3(corpsePost.x, corpsePost.y, corpsePost.z), Quaternion.Euler(new Vector3(0f, 0f)));
        Instantiate(corpse.gameObject, new Vector3(corpsePost.x,corpsePost.y, corpsePost.z), Quaternion.Euler(new Vector3(0f, 90f)));
        // А теперь убираем персонажа
        Destroy(this.gameObject);
    }

    public virtual bool IsAlive()
    {
        return curHealth > 0;
    }

    /// <summary>
    /// Вызывается в момент удара оружием
    /// </summary>
    public virtual void Hit()
    {
        if (attackTarget != null && attackTarget.GetComponent<IAttackable>() != null)
        {
            attackTarget.GetComponent<IAttackable>().GetHit(gameObject, attack);
        }
    }

    /// <summary>
    /// Вызывается в момент окончания атаки
    /// </summary>
    public virtual void EndAttack()
    {
        animator.SetBool(attackBool, false);
    }

    public virtual void StunForTime(float stunTime)
    {
        stunDuration = stunTime;
        GetHit(null,0);
    }


    /// <summary>
    /// Поиск ближайшей цели
    /// </summary>
    public void GetClosestTarget()
    {
        if(attackTargets.Count == 0)
        {
            return;
        }

        if (agent.isOnNavMesh)
        {
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject go in attackTargets)
            {
                if (go != null)
                {
                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        attackTarget = go;
                        distance = curDistance;
                    }
                }
            }
        }
        else
        {
            // Костыль из за особенностей юнити
            // Чаще всего актуален для тараканов, но при усложнении системы может быть полезен везде
            // Фишка в том, что, например, если миньон появляется в воздухе, то без рестарта navmeshagent, так и не будет засчитано приземление
            agent.enabled = false;
            agent.enabled = true;
        }
    }
}
