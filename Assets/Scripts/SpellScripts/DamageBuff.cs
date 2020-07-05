using UnityEngine;

/// <summary>
/// Баф к получаемому урону
/// </summary>
public abstract class DamageBuff : MonoBehaviour
{
    /// <summary>
    /// Количество дополнительного урона
    /// </summary>
    /// <param name="baseDamage">базовый получаемый урон</param>
    public abstract int GetAdditionalDamage(int baseDamage);
}
