using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Объект, с которым можно взаимодействовать
/// </summary>
public interface Interactable
{
    /// <summary>
    /// Можно ли взаимодействовать с объектом
    /// </summary>
    bool CanInteract();

    /// <summary>
    /// Получить текст действия
    /// </summary>
    /// <returns>строку, которая будет отображена в интерфейсе</returns>
    string GetActionText();

    /// <summary>
    /// Взаимодействие с объектом
    /// </summary>
    void DoInteraction();
}
