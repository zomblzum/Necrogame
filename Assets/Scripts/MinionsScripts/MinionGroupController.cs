using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinionGroupController : MonoBehaviour
{
    [Header("Активное поведение прислужников в команде")]
    public MinionCommand minionCommand;
    [Header("Цель движения для группы")]
    public Vector3 movePosition;
    [Header("Цель атаки для группы")]
    public GameObject attackTarget;

    protected MinionBehaviour.MinionGroup minionGroup;
    protected bool groupUnderControl;

    public virtual void SetGroupToController(MinionBehaviour.MinionGroup minionGroup)
    {
        this.minionGroup = minionGroup;
        this.groupUnderControl = true;
    }

    /// <summary>
    /// Тригер при добавлении миньона
    /// </summary>
    public abstract void MinionAdded();

    /// <summary>
    /// Тригер при удалении миньона
    /// </summary>
    public abstract void MinionRemoved();

    /// <summary>
    /// Заставить всех миньонов подчинятся
    /// </summary>
    public virtual void SetGroupUnderControl()
    {
        foreach (Minion minion in minionGroup.minions)
        {
            if (!minion.underControl)
            {
                minion.GoUnderControl();
            }
        }
        groupUnderControl = true;
    }

    /// <summary>
    /// Заставить всех миньонов бунтовать
    /// </summary>
    public virtual void SetGroupOutControl()
    {
        foreach (Minion minion in minionGroup.minions)
        {
            if (minion.underControl)
            {
                minion.GoOutControl();
            }
        }
        groupUnderControl = false;
    }

    /// <summary>
    /// Команда атаки
    /// </summary>
    public virtual void SetGroupAttackTarget(GameObject attackTarget)
    {
        this.attackTarget = attackTarget;
        foreach(Minion minion in minionGroup.minions)
        {
            minion.SetAttackTarget(attackTarget);
        }
    }

    /// <summary>
    /// Команда передвигаться к нужной позиции
    /// </summary>
    public virtual void SetGroupMoveTarget(Vector3 movePosition)
    {
        this.movePosition = movePosition;
        foreach (Minion minion in minionGroup.minions)
        {
            minion.inAggro = false;
            minion.SetMoveTarget(movePosition);
            //minion.ReturnToDefaultPosition();
        }
    }

    /// <summary>
    /// Поменять текущее поведение прислужников
    /// </summary>
    public virtual void ChangeCurrentCommand(MinionCommand minionCommand)
    {
        this.minionCommand = minionCommand;
        foreach (Minion minion in minionGroup.minions)
        {
            minion.SetCommandBehaviour(minionCommand);
        }
    }
}
