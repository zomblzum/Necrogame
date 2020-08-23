using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Отдельный менеджер для управления всеми группами сразу
/// Позволяет распределять разные типы миньонов на разные позиции
/// </summary>
public class AllGroupPositioner : MinionGroupPositioner
{
    public override GameObject GetMovePositionForMinion(int minionId, string minionType)
    {
        if (minionType == "Demon" || minionType == "Roach")
        {
            return GetMoveObject(minionId);
        }
        else
        {
            return GetMoveObject(10 + minionId);
        }
    }

    public override GameObject GetDefendPositionForMinion(int minionId, string minionType)
    {
        if (minionType == "Demon" || minionType == "Roach")
        {
            return GetDefendObject(minionId);
        }
        else
        {
            return GetDefendObject(10 +  minionId);
        }
    }
}
