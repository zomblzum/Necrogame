using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Minion : Character
{
    [Header("Подчиняется игроку в данный момент")]
    public bool underControl = true;
    [Header("Теги персонажей, на которые может осуществлятся нападение")]
    public List<string> targetsTags;

    [SerializeField]
    [Header("Объект, для определения координат при команде разбежаться")]
    protected MinionFleeObject fleeTarget;
    [SerializeField]
    [Header("Радиус области побега")]
    protected float fleeAreaRaidus;

    [SerializeField]
    [Header("Область, защищаемая миньоном")]
    protected DefendArea defendArea;
    [SerializeField]
    [Header("Радиус защищаемой области")]
    protected float defendAreaRaidus;

    protected GameObject moveTarget;
    protected MinionBehaviour minionBehaviour;
    protected MinionCommand minionCommand;
    protected Player player;

    protected virtual void Awake()
    {
        inAggro = false;
        player = FindObjectOfType<Player>();
        minionBehaviour = FindObjectOfType<MinionBehaviour>();
        attackTargets = new List<GameObject>();
        //спавн для защищаемой зоны, чтобы он был не дочерним обьектом
        defendArea = Instantiate(defendArea, transform.position, transform.rotation);
        defendArea.SetMinion(this);
        defendArea.SetAreaRadius(defendAreaRaidus);
        //спавн для объекта, определяющего куда бежать при разбеге
        fleeTarget = Instantiate(fleeTarget, transform.position, transform.rotation);
        fleeTarget.SetMinion(this);
        fleeTarget.SetAreaRadius(fleeAreaRaidus);
    }

    protected override void CharacterBehaviour()
    {
        if(minionCommand.commandName == "Attack")
        {
            AttackCommand();
        } 
        else if (minionCommand.commandName == "Move")
        {
            MoveCommand();
        }
        else if (minionCommand.commandName == "Defend")
        {
            DefendCommand();
        }
        else if (minionCommand.commandName == "Disgroup")
        {
            DisgroupCommand();
        }
    }

    protected override void Update()
    {
        if(minionCommand.commandName == "Move" || minionCommand.commandName == "Defend")
        {
            if(attackTargets.Count != defendArea.EnemysCount())
            {
                attackTargets = defendArea.GetEnemies();
            }
            base.Update();
        }
    }

    /// <summary>
    /// Реализация поведения при команде АТАКИ
    /// </summary>
    protected virtual void AttackCommand()
    {
        if (attackTarget != null)
        {
            AttackTargetInteractionsBehaviour();
        } 
        else
        {
            if(defendArea.EnemysCount() > 0)
            {
                attackTargets = defendArea.GetEnemies();
                GetClosestTarget();
            } else
            {
                SetCommandBehaviour(new MinionCommand("Move"));
            }
            //FindTargetsByTags();
            //GetClosestTarget();
        }
    }

    /// <summary>
    /// Реализация поведения при команде ПЕРЕМЕЩЕНИЯ К ТОЧКЕ
    /// </summary>
    protected virtual void MoveCommand()
    {
        if (moveTarget)
        {
            SetMovePoint(moveTarget.transform.position);
        }
        if (movePoint != Vector3.zero)
        {
            MovingBehaviour();
        }
        if (attackTarget != null && inAggro)
        {
            AttackTargetInteractionsBehaviour();
        }
    }

    /// <summary>
    /// Реализация поведения при команде ЗАЩИТЫ ИГРОКА
    /// </summary>
    protected virtual void DefendCommand()
    {
        MoveCommand();
    }

    /// <summary>
    /// Возврат к цели перемещения(например после атаки)
    /// </summary>
    public virtual void ReturnToDefaultPosition()
    {
        if (moveTarget != null && underControl && (minionCommand.commandName == "Move" || minionCommand.commandName == "Defend"))
        {
            attackTarget = null;
            SetMovePoint(moveTarget.transform.position);
        }
    }

    /// <summary>
    /// Реализация поведения при команде РАЗГРУПИРОВКИ
    /// </summary>
    protected virtual void DisgroupCommand()
    {
        inAggro = false;
        
        if (fleeTarget.NeedToFlee())
        {
            SetMovePoint(fleeTarget.GetFleeCoordinates());
            MoveCommand();
        }
        else if (movePoint == Vector3.zero || !fleeTarget.NeedToFlee())
        {
            PassiveBehaviour();
        }
    }

    public override void Die(string deathText)
    {
        minionBehaviour.RemoveMinion(this);
        Destroy(defendArea.gameObject);
        Destroy(fleeTarget.gameObject);
        base.Die(deathText);
    }

    protected override void RunToEnemy()
    {
        base.RunToEnemy();
        MovingBehaviour();
    }

    /// <summary>
    /// Задать активную команду для прислужника
    /// </summary>
    public virtual void SetCommandBehaviour(MinionCommand minionCommand)
    {
        this.minionCommand = minionCommand;
    }

    /// <summary>
    /// Выбираем текущий список цели по тегам врагом
    /// </summary>
    public virtual void FindTargetsByTags()
    {
        attackTargets.Clear();
        foreach(string tag in targetsTags)
        {
            attackTargets.AddRange(GameObject.FindGameObjectsWithTag(tag));
        }
        attackTargets.Remove(this.gameObject);
    }

    /// <summary>
    /// Выход из под контроля
    /// </summary>
    public abstract void GoOutControl();

    /// <summary>
    /// Возвращение под контроль
    /// </summary>
    public abstract void GoUnderControl();

    public void SetMoveTarget(GameObject moveTarget)
    {
        SetMovePoint(moveTarget.transform.position);
        this.moveTarget = moveTarget;
    }
}
