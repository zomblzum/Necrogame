using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinionGroupController : MonoBehaviour
{
    protected MinionBehaviour.MinionGroup minionGroup;
    protected bool groupUnderControl;

    /// <summary>
    /// Тригер при добавлении миньона
    /// </summary>
    public abstract void MinionAdded();

    /// <summary>
    /// Тригер при удалении миньона
    /// </summary>
    public abstract void MinionRemoved();

    public virtual void SetGroupToController(MinionBehaviour.MinionGroup minionGroup)
    {
        this.minionGroup = minionGroup;
        this.groupUnderControl = true;
    }

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
    public abstract void AttackCommand();

    /// <summary>
    /// Команда защищать игрока
    /// </summary>
    public abstract void DefendCommand();

    /// <summary>
    /// Команда передвигаться к нужной позиции
    /// </summary>
    public abstract void MoveCommand(Vector3 position);

    /// <summary>
    /// Команда рассредоточиться
    /// </summary>
    public abstract void DisgroupCommand();
}
