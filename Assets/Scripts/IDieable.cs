/// <summary>
/// Объект может умереть
/// </summary>
public interface IDieable
{
    /// <summary>
    /// Смерть
    /// </summary>
    void Die();


    /// <summary>
    /// Состояние объекта
    /// </summary>
    bool IsAlive();
}
