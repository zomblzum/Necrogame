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
}
