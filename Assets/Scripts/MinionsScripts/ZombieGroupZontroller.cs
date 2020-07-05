using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGroupZontroller : MinionGroupController
{
    public override void AttackCommand()
    {
        //throw new System.NotImplementedException();
    }

    public override void DefendCommand()
    {
        //throw new System.NotImplementedException();
    }

    public override void DisgroupCommand()
    {
        //throw new System.NotImplementedException();
    }

    public override void MoveCommand(Vector3 position)
    {
        //throw new System.NotImplementedException();
    }

    public override void MinionAdded()
    {
        if(minionGroup.minions.Count > minionGroup.maxMinions)
        {
            SetGroupOutControl();
        }
    }

    public override void MinionRemoved()
    {
        if (minionGroup.minions.Count <= minionGroup.maxMinions)
        {
            SetGroupUnderControl();
        }
    }
}
