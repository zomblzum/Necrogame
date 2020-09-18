using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerFleeObject : FleeObject
{
    public override bool CorrectTarget(GameObject target)
    {
        return target.GetComponent<Minion>() || target.GetComponent<Player>();
    }
}
