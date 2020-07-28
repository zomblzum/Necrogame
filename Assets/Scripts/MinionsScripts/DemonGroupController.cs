using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonGroupController : MinionGroupController
{
    [Header("Время прислуживания, после которого демон выйдет из под контроля")]
    public float serveTime = 180f;

    private float curTime = 0f;
    private float calculatedServeTime = 100f;


    public override void MinionAdded()
    {
        CalculateTime();
    }

    public override void MinionRemoved()
    {
        CalculateTime();
    }

    private void CalculateTime()
    {
        calculatedServeTime = serveTime - (minionGroup.minions.Count * (serveTime / 100 * 20));
    }

    // Update is called once per frame
    void Update()
    {
        if (curTime >= calculatedServeTime && groupUnderControl)
        {
            SetGroupOutControl();
        }
        else if (minionGroup.minions.Count > 0)
        {
            curTime += Time.deltaTime;
        }
    }

    public override void ChangeCurrentCommand(MinionCommand minionCommand)
    {
        base.ChangeCurrentCommand(minionCommand);
        RestartTriggerTime();
    }

    private void RestartTriggerTime()
    {
        curTime = 0;
        SetGroupUnderControl();
    }
}
