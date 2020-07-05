using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSummoner : Summoner
{
    [Header("Требуемое количество здоровья")]
    public int manaCost = 10;

    public override void TakeCost(Player player)
    {
        player.SpendMana(manaCost);
    }


    public override bool CanSummon(Player player)
    {
        return player.curMana >= manaCost;
    }
}
