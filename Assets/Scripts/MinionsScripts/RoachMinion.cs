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
        Die();
    }

    public override void GoUnderControl()
    {
        Die();
    }

    private void Update()
    {
        curTime += Time.deltaTime;

        if(curTime >= lifeTime)
        {
            Die();
        }
    }
}
