using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGroupZontroller : MinionGroupController
{
    public MinionBehaviour minionBehaviour;

    public override void MinionAdded(Minion minion)
    {
        base.MinionAdded(minion);
        if(minionBehaviour.minionGroups[0].minions.Count > minionBehaviour.maxMinions)
        {
            SetGroupOutControl();
        }
    }

    public override void MinionRemoved()
    {
        if (minionBehaviour.minionGroups[0].minions.Count <= minionBehaviour.maxMinions)
        {
            SetGroupUnderControl();
        }
    }
}
