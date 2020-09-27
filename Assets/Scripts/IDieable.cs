/// <summary>
/// Объект может умереть
/// </summary>
public interface IDieable
{
    /// <summary>
    /// Смерть
    /// </summary>
    void Die(string deathText);


    /// <summary>
    /// Состояние объекта
    /// </summary>
    bool IsAlive();
}
