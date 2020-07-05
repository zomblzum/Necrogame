/// <summary>
/// Объект можно оглушить
/// </summary>
public interface IStunable
{
    /// <summary>
    /// Оглушить на время
    /// </summary>
    /// <param name="stunTime">время оглушения в секундах</param>
    void StunForTime(float stunTime);
}
