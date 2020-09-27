using UnityEngine;

/// <summary>
/// Таракан
/// Умирает спустя какое то время
/// </summary>
public class RoachMinion : Minion
{
    [Header("Время жизни в секундах")]
    public float lifeTime = 180f;

    private float curTime = 0f;

    public override void GoOutControl()
    {
        Die("-Roach");
    }

    public override void GoUnderControl()
    {
        Die("-Roach");
    }

    protected override void Update()
    {
        base.Update();
        curTime += Time.deltaTime;

        if(curTime >= lifeTime)
        {
            Die("-Roach");
        }
    }
}
