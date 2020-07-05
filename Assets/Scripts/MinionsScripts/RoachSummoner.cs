using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoachSummoner : Summoner
{
    [Header("Список яиц для вылупления")]
    public List<RoachEgg> eggs;

    public void Awake()
    {
        foreach(RoachEgg egg in eggs)
        {
            egg.gameObject.SetActive(false);
        }
    }

    public override void TakeCost(Player player)
    {
        
    }

    public override bool CanSummon(Player player)
    {
        return GetFreeEgg() != null;
    }

    protected override void InstantiateMinion(Minion minion)
    {
        base.InstantiateMinion(minion);
        GetFreeEgg().StartHatch((RoachMinion) minionToSummon);
    }

    public RoachEgg GetFreeEgg()
    {
        for(int i = 0; i < eggs.Count; i++)
        {
            if(!eggs[i].gameObject.activeSelf)
            {
                return eggs[i];
            }
        }
        return null;
    }
}
