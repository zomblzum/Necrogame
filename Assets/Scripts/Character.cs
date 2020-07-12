using System.Collections;
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

    protected CharacterUI characterUI;
    protected Animator animator;
    protected GameObject attackTarget;
    protected List<GameObject> attackTargets;
    protected float stunDuration;
    protected int attackSpeedFloat;
    protected int attackBool;
    protected int speedFloat;
    protected int curHealth;
    protected int hitBool;
    protected int stunBool;


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
    }

    /// <summary>
    /// Назначить приоритетную цель
    /// </summary>
    public void SetAttackTarget(GameObject attackable)
    {
        attackTarget = attackable;
    }

    /// <summary>
    /// Текущая цель для атаки
    /// </summary>
    public GameObject GetCurrentTarget()
    {
        return attackTarget;
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
    /// Атакуем игрока
    /// </summary>
    public virtual void Attack()
    {
        if (stunDuration <= 0 && inAggro)
        {
            animator.SetBool(attackBool, true);
            animator.SetFloat(attackSpeedFloat, attackSpeed);
        }
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
        if (!inAggro || stunDuration > 0)
        {
            PassiveBehaviour();
        } 
        else if (IsAlive() && attackTarget != null)
        {
            TargetInteractionsBehaviour();
        }
    }

    /// <summary>
    /// Поведение персонажа, когда нет цели или в стане
    /// </summary>
    protected virtual void PassiveBehaviour()
    {
        agent.SetDestination(transform.position);
        agent.velocity = Vector3.zero;
        agent.isStopped = true;

        animator.SetFloat(speedFloat, 0f);
    }

    /// <summary>
    /// Передвижение к цели и ее атака
    /// </summary>
    protected virtual void TargetInteractionsBehaviour()
    {
        agent.SetDestination(attackTarget.transform.position);
        if (Vector3.Distance(transform.position, attackTarget.transform.position) <= attackDistance)
        {
            transform.LookAt(attackTarget.transform);
            PassiveBehaviour();
            if (!animator.GetBool(attackBool))
            {
                animator.SetFloat(speedFloat, 0f);
                Attack();
            }
        }
        else
        {
            agent.isStopped = false;
            animator.SetFloat(speedFloat, 1f);
        }
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
        if (agent.isOnNavMesh)
        {
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject go in attackTargets)
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
