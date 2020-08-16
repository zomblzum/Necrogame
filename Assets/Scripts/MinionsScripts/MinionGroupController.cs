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
        //foreach (Minion minion in minionGroup.minions)
        //{
        //    minion.inAggro = false;
        //    minion.SetDefendPoint(movePosition);
        //    //minion.ReturnToDefaultPosition();
        //}

        for(int i = 0; i < minionGroup.minions.Count; i++)
        {
            minionGroup.minions[i].inAggro = false;
            minionGroup.minions[i].SetDefendPoint(GetMovePositionForMinion(i,movePosition));
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
    /// Расчет места в формации при атаке
    /// </summary>
    /// <param name="minionId">номер миньона в списке</param>
    /// <returns>позиция конкретного миньона в отряде</returns>
    public virtual Vector3 GetAttackPositionForMinion(int minionId, Vector3 originalPosition)
    {
        return originalPosition + minionGroupPositioner.GetMovePosition(minionId);
    }

    /// <summary>
    /// Расчет места в формации при движении к точке
    /// </summary>
    /// <param name="minionId">номер миньона в списке</param>
    /// <returns>позиция конкретного миньона в отряде</returns>
    public virtual Vector3 GetMovePositionForMinion(int minionId, Vector3 originalPosition)
    {
        Debug.Log("Оригинал " + originalPosition + " Добавляем " + minionGroupPositioner.GetMovePosition(minionId));
        return originalPosition + minionGroupPositioner.GetMovePosition(minionId);
    }

    /// <summary>
    /// Расчет места в формации при защите игрока
    /// </summary>
    /// <param name="minionId">номер миньона в списке</param>
    /// <returns>позиция конкретного миньона в отряде</returns>
    public virtual Vector3 GetDefendPositionForMinion(int minionId, Vector3 originalPosition)
    {
        return originalPosition + minionGroupPositioner.GetMovePosition(minionId);
    }

    /// <summary>
    /// Расчет места в формации при команде рассредоточения
    /// </summary>
    /// <param name="minionId">номер миньона в списке</param>
    /// <returns>позиция конкретного миньона в отряде</returns>
    public virtual Vector3 GetDisgroupPositionForMinion(int minionId, Vector3 originalPosition)
    {
        return originalPosition + minionGroupPositioner.GetMovePosition(minionId);
    }
}
