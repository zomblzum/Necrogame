using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllGroupController : MinionGroupController
{
    [Header("Все группы миньонов")]
    public List<MinionGroupController> minionGroupControllers;


    public override void MinionAdded()
    {
        foreach (MinionGroupController controller in minionGroupControllers)
        {
            controller.MinionAdded();
        }
    }

    public override void MinionRemoved()
    {
        foreach (MinionGroupController controller in minionGroupControllers)
        {
            controller.MinionRemoved();
        }
    }

    public override void SetGroupMoveTarget(Vector3 movePosition)
    {
        foreach (MinionGroupController controller in minionGroupControllers)
        {
            controller.SetGroupMoveTarget(movePosition);
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

    public override Vector3 GetMovePositionForMinion(int minionId, Vector3 originalPosition)
    {
        string minionType = minionGroup.minions[minionId].characterName;

        foreach (MinionGroupController controller in minionGroupControllers)
        {
            if(controller.GetMinionType() == minionType)
            {
                return controller.GetMovePositionForMinion(minionId, originalPosition);
            }
        }

        return Vector3.zero;
    }

    public override Vector3 GetAttackPositionForMinion(int minionId, Vector3 originalPosition)
    {
        string minionType = minionGroup.minions[minionId].characterName;

        foreach (MinionGroupController controller in minionGroupControllers)
        {
            if (controller.GetMinionType() == minionType)
            {
                return controller.GetAttackPositionForMinion(minionId, originalPosition);
            }
        }

        return Vector3.zero;
    }

    public override Vector3 GetDefendPositionForMinion(int minionId, Vector3 originalPosition)
    {
        string minionType = minionGroup.minions[minionId].characterName;

        foreach (MinionGroupController controller in minionGroupControllers)
        {
            if (controller.GetMinionType() == minionType)
            {
                return controller.GetDefendPositionForMinion(minionId, originalPosition);
            }
        }

        return Vector3.zero;
    }

    public override Vector3 GetDisgroupPositionForMinion(int minionId, Vector3 originalPosition)
    {
        string minionType = minionGroup.minions[minionId].characterName;

        foreach (MinionGroupController controller in minionGroupControllers)
        {
            if (controller.GetMinionType() == minionType)
            {
                return controller.GetDisgroupPositionForMinion(minionId, originalPosition);
            }
        }

        return Vector3.zero;
    }

}
