using UnityEngine;

/// <summary>
/// Модификатор урона от яиц тараканов
/// </summary>
public class RoachEggsDamageBuff : DamageBuff
{
    private int activeEggsCount = 0;

    public override int GetAdditionalDamage(int baseDamage)
    {
        //Преобразования во float и обратно для того, чтобы не потерять разделенные значения после запятой
        return (int)(activeEggsCount *((float)baseDamage/100*20));
    }

    public void EggHatchStart()
    {
        activeEggsCount += 1;
    }

    public void EggHatchEnd()
    {
        activeEggsCount -= 1;
    }
}
