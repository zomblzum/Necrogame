using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditGroupController : MinionGroupController
{
    [Header("Промежуток для списания денег в секундах")]
    public float takeMoneyTime = 180f;
    [Header("Списываемые со временем деньги у игрока")]
    public int price = 20;
    [Header("Игрок")]
    public Player player;

    private float curTime = 0f;
    private float calculatedTime;

    public override void MinionAdded(Minion minion)
    {
        base.MinionAdded(minion);
        CalculateTime();

        if(!groupUnderControl)
        {
            SetGroupOutControl();
        }
    }

    public override void MinionRemoved()
    {
        CalculateTime();
    }

    private void CalculateTime()
    {
        curTime = 0f;
        calculatedTime = takeMoneyTime / minionGroup.minions.Count;
    }

    public override void SetGroupUnderControl()
    {
        if(!groupUnderControl && player.money >= price * minionGroup.minions.Count)
        {
            player.SpendMoney(price * minionGroup.minions.Count);
            curTime = 0f;
            base.SetGroupUnderControl();
        }
    }

    void Update()
    {
        if (curTime >= calculatedTime && minionGroup.minions.Count > 0 && groupUnderControl)
        {
            Debug.Log("Стрелки вышел из под контроля");
            SetGroupOutControl();
        } 
        else
        {
            curTime += Time.deltaTime;
        }
    }
}
