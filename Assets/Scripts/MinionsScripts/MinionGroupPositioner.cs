using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Менеджер для управления формациями
/// Не самый ООП код, но нет смысла делать это интерфейсом и делать реализации
/// т.к. не будет общего шаблона тогда
/// </summary>
public class MinionGroupPositioner: MonoBehaviour
{
    [Header("Формация отряда при движении")]
    public MinionFormation moveFormation;
    [Header("Формация отряда для защиты объекта")]
    public MinionFormation defendFormation;
    [Header("Формация отряда при разгрупировке")]
    public MinionFormation disgroupFormation;

    public void SetMoveFormationPosition(Vector3 position)
    {
        moveFormation.transform.LookAt(position);
        moveFormation.transform.position = position;
    }

    public void SetDisgroupFormationPosition(Vector3 position)
    {
        disgroupFormation.transform.LookAt(position);
        disgroupFormation.transform.position = position;
    }

    public virtual GameObject GetMoveObject(int minionId)
    {
        return moveFormation.GetPosition(minionId);
    }

    public virtual GameObject GetDefendObject(int minionId)
    {
        return defendFormation.GetPosition(minionId);
    }

    public virtual GameObject GetDisgroupObject(int minionId)
    {
        return disgroupFormation.GetPosition(minionId);
    }

    public virtual GameObject GetDefendPositionForMinion(int minionId, string minionType) 
    {
        return GetDefendObject(minionId);
    }

    public virtual GameObject GetMovePositionForMinion(int minionId, string minionType) 
    {
        return GetMoveObject(minionId);
    }
}