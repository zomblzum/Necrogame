using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSummoner : Summoner
{
    [Header("Требуемое количество маны")]
    public int healthCost = 10;

    public override void TakeCost(Player player)
    {
        player.GetHit(player.gameObject, healthCost);
    }

    public override bool CanSummon(Player player)
    {
        return player.curHealth >= healthCost;
    }
}
