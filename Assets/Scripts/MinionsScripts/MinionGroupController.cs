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
    [Header("Модуль для позиционирования миньонов в отряде")]
    public MinionGroupPositioner minionGroupPositioner;

    protected MinionBehaviour.MinionGroup minionGroup;
    protected bool groupUnderControl;

    public virtual void SetGroupToController(MinionBehaviour.MinionGroup minionGroup)
    {
        this.minionGroup = minionGroup;
        this.groupUnderControl = true;
    }

    public virtual string GetMinionType()
    {
        return minionGroup.minionType;
    }

    /// <summary>
    /// Тригер при добавлении миньона
    /// </summary>
    public virtual void MinionAdded(Minion minion)
    {
        if (minionCommand.commandName == "Attack")
        {
            minion.SetAttackTarget(attackTarget);
        }
        else if (minionCommand.commandName == "Move")
        {
            minion.SetMovePoint(movePosition);
        }
        else if (minionCommand.commandName == "Defend")
        {
            SetGroupDefendPlayer();
        }
        else if (minionCommand.commandName == "Disgroup")
        {
            
        }
    }

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
    public virtual void SetGroupMovePoint(Vector3 movePosition)
    {
        this.movePosition = movePosition;

        minionGroupPositioner.SetMoveFormationPosition(movePosition);
        //minionGroupPositioner.SetDisgroupFormationPosition(movePosition);
        for (int i = 0; i < minionGroup.minions.Count; i++)
        {
            minionGroup.minions[i].inAggro = false;
            minionGroup.minions[i].SetMoveTarget(GetMovePositionForMinion(i));
        }
    }

    /// <summary>
    /// Команда передвигаться за игроком
    /// </summary>
    public virtual void SetGroupDefendPlayer()
    {
        for (int i = 0; i < minionGroup.minions.Count; i++)
        {
            minionGroup.minions[i].inAggro = false;
            minionGroup.minions[i].SetMoveTarget(GetDefendPositionForMinion(i));
        }
    }

    /// <summary>
    /// Команда разбежаться
    /// </summary>
    public virtual void DisgroupMinions()
    {
        for (int i = 0; i < minionGroup.minions.Count; i++)
        {
            minionGroup.minions[i].inAggro = false;
            minionGroup.minions[i].SetMoveTarget(GetDisgroupPositionForMinion(i));
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

    /// <summary>
    /// Расчет места в формации при движении к точке
    /// </summary>
    /// <param name="minionId">номер миньона в списке</param>
    /// <returns>позиция конкретного миньона в отряде</returns>
    public virtual GameObject GetMovePositionForMinion(int minionId)
    {
        return minionGroupPositioner.GetMoveObject(minionId);
    }

    /// <summary>
    /// Расчет места в формации при защите игрока
    /// </summary>
    /// <param name="minionId">номер миньона в списке</param>
    /// <returns>позиция конкретного миньона в отряде</returns>
    public virtual GameObject GetDefendPositionForMinion(int minionId)
    {
        return minionGroupPositioner.GetDefendObject(minionId);
    }

    /// <summary>
    /// Расчет места в формации при команде разбежаться
    /// <param name="minionId">номер миньона в списке</param>
    /// <returns>позиция конкретного миньона в отряде</returns>
    public virtual GameObject GetDisgroupPositionForMinion(int minionId)
    {
        return minionGroupPositioner.GetDisgroupObject(minionId);
    }

    public virtual int GetMinionsCount()
    {
        return minionGroup.minions.Count;
    }
}
