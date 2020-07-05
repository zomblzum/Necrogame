using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditSummoner : Summoner
{
    [Header("Требуемое количество маны")]
    public int price = 10;

    public override bool CanSummon(Player player)
    {
        return player.money >= price;
    }

    public override void TakeCost(Player player)
    {
        player.SpendMoney(price);
    }
}
