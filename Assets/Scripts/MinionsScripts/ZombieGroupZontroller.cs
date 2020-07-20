using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGroupZontroller : MinionGroupController
{
    public MinionBehaviour minionBehaviour;

    public override void MinionAdded()
    {
        if(minionGroup.minions.Count > minionBehaviour.maxMinions)
        {
            SetGroupOutControl();
        }
    }

    public override void MinionRemoved()
    {
        if (minionGroup.minions.Count <= minionBehaviour.maxMinions)
        {
            SetGroupUnderControl();
        }
    }
}
