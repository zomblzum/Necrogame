using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllGroupController : MinionGroupController
{
    [Header("Все группы миньонов")]
    public List<MinionGroupController> minionGroupControllers;

    public override void AttackCommand()
    {
        foreach(MinionGroupController controller in minionGroupControllers)
        {
            controller.AttackCommand();
        }
    }

    public override void DefendCommand()
    {
        foreach (MinionGroupController controller in minionGroupControllers)
        {
            controller.DefendCommand();
        }
    }

    public override void DisgroupCommand()
    {
        foreach (MinionGroupController controller in minionGroupControllers)
        {
            controller.DisgroupCommand();
        }
    }

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

    public override void MoveCommand(Vector3 position)
    {
        foreach (MinionGroupController controller in minionGroupControllers)
        {
            controller.MoveCommand(position);
        }
    }
}
