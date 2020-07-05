using UnityEngine;
/// <summary>
/// Объект имеет свое поведение при получении урона
/// </summary>
public interface IAttackable
{
    /// <summary>
    /// Получить урон
    /// </summary>
    /// <param name="damage">количество урона</param>
    void GetHit(GameObject attackFrom, int damage);
}
