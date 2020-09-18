using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllGroupController : MinionGroupController
{
    [Header("Все группы миньонов")]
    public List<MinionGroupController> minionGroupControllers;

    public override void MinionRemoved()
    {
        foreach (MinionGroupController controller in minionGroupControllers)
        {
            controller.MinionRemoved();
        }
    }

    public override void SetGroupMovePoint(Vector3 movePosition)
    {
        base.SetGroupMovePoint(movePosition);
        foreach (MinionGroupController controller in minionGroupControllers)
        {
            controller.minionGroupPositioner.SetMoveFormationPosition(movePosition);
        }
    }

    //public override void SetGroupDefendPlayer()
    //{
    //    foreach (MinionGroupController controller in minionGroupControllers)
    //    {
    //        controller.SetGroupDefendPlayer();
    //    }
    //}

    public override void DisgroupMinions()
    {
        foreach (MinionGroupController controller in minionGroupControllers)
        {
            controller.DisgroupMinions();
        }
    }

    public override void SetGroupAttackTarget(GameObject attackTarget)
    {
        foreach (MinionGroupController controller in minionGroupControllers)
        {
            controller.SetGroupAttackTarget(attackTarget);
        }
    }

    public override void SetGroupOutControl()
    {
        foreach (MinionGroupController controller in minionGroupControllers)
        {
            controller.SetGroupOutControl();
        }
    }

    public override void SetGroupUnderControl()
    {
        foreach (MinionGroupController controller in minionGroupControllers)
        {
            controller.SetGroupUnderControl();
        }
    }

    public override void ChangeCurrentCommand(MinionCommand minionCommand)
    {
        base.ChangeCurrentCommand(minionCommand);
        foreach (MinionGroupController controller in minionGroupControllers)
        {
            controller.ChangeCurrentCommand(minionCommand);
        }
    }

    public override GameObject GetMovePositionForMinion(int minionId)
    {
        Minion minion = minionGroup.minions[minionId];
        int minionTypesCount = 0;
        GameObject result = null;

        foreach (MinionGroupController controller in minionGroupControllers)
        {
            if (controller.GetMinionsCount() > 0)
            {
                minionTypesCount++;
            }
            if (controller.GetMinionType() == minion.characterName)
            {
                result = controller.GetMovePositionForMinion(minionId);
            }
        }

        if (result != null && minionTypesCount == 1)
        {
            return result;
        }
        else
        {
            return minionGroupPositioner.GetMovePositionForMinion(minionId, minion.characterName);
        }
    }

    public override GameObject GetDefendPositionForMinion(int minionId)
    {
        Minion minion = minionGroup.minions[minionId];
        int minionTypesCount = 0;
        GameObject result = null;

        foreach (MinionGroupController controller in minionGroupControllers)
        {
            if (controller.GetMinionsCount() > 0)
            {
                minionTypesCount++;
            }
            if (controller.GetMinionType() == minion.characterName)
            {
                result = controller.GetDefendPositionForMinion(minionId);
            }
        }

        if(result != null && minionTypesCount == 1)
        {
            return result;
        }
        else
        {
            return minionGroupPositioner.GetDefendPositionForMinion(minionId, minion.characterName);
        }
    }


}
