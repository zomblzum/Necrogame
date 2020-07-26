using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Minion : Character
{
    [Header("Подчиняется игроку в данный момент")]
    public bool underControl = true;
    [Header("Теги персонажей, на которые может осуществлятся нападение")]
    public List<string> targetsTags;

    protected MinionBehaviour minionBehaviour;
    protected MinionCommand minionCommand;
    protected Player player;
    
    protected virtual void Awake()
    {
        player = FindObjectOfType<Player>();
        minionBehaviour = FindObjectOfType<MinionBehaviour>();
        attackTargets = new List<GameObject>();
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

    /// <summary>
    /// Реализация поведения при команде АТАКИ
    /// </summary>
    protected virtual void AttackCommand()
    {
        if (attackTarget != null)
        {
            AttackTargetInteractionsBehaviour();
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
    }

    /// <summary>
    /// Реализация поведения при команде ЗАЩИТЫ ИГРОКА
    /// </summary>
    protected virtual void DefendCommand()
    {
        SetMoveTarget(player.transform.position);
        MoveCommand();
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
}
