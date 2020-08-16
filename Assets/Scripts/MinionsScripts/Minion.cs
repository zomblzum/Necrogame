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
    [Header("Область, защищаемая миньоном")]
    protected DefendArea defendArea;
    [SerializeField]
    [Header("Радиус защищаемой области")]
    protected float defendAreaRaidus;


    protected MinionBehaviour minionBehaviour;
    protected MinionCommand minionCommand;
    protected Player player;
    protected Vector3 localMoveTarget; //нужна для возврата к указанной игроком точке после отбегания от нее
    
    protected virtual void Awake()
    {
        inAggro = false;
        player = FindObjectOfType<Player>();
        minionBehaviour = FindObjectOfType<MinionBehaviour>();
        attackTargets = new List<GameObject>();
        localMoveTarget = Vector3.zero;
        //костыль для защищаемой зоны, чтобы он был не дочерним обьектом
        defendArea = Instantiate(defendArea, transform.position, transform.rotation);
        defendArea.SetMinion(this);
        defendArea.SetAreaRadius(defendAreaRaidus);
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
            }
            SetCommandBehaviour(new MinionCommand("Move"));
            //FindTargetsByTags();
            //GetClosestTarget();
        }
    }

    /// <summary>
    /// Реализация поведения при команде ПЕРЕМЕЩЕНИЯ К ТОЧКЕ
    /// </summary>
    protected virtual void MoveCommand()
    {
        if (moveTarget != Vector3.zero)
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
        SetMoveTarget(player.transform.position);
        MoveCommand();
    }

    public void ReturnToDefaultPosition()
    {
        if (minionCommand.commandName == "Move" || minionCommand.commandName == "Defend")
        {
            attackTarget = null;
            SetMoveTarget(localMoveTarget);
            localMoveTarget = Vector3.zero;
        }
    }

    /// <summary>
    /// Реализация поведения при команде РАЗГРУПИРОВКИ
    /// </summary>
    protected virtual void DisgroupCommand()
    {

    }

    public override void Die()
    {
        minionBehaviour.RemoveMinion(this);
        base.Die();
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

    public override void SetMoveTarget(Vector3 moveTarget)
    {
        base.SetMoveTarget(moveTarget);
    }

    public virtual void SetDefendPoint(Vector3 defendPoint)
    {
        localMoveTarget = moveTarget;
        SetMoveTarget(defendPoint);
    }
}
