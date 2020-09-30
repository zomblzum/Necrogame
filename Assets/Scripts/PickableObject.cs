using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Подбираемый предмет
/// </summary>
public abstract class PickableObject : MonoBehaviour
{
    [Header("Уничтожать после подбора")]
    public bool destroyAfterPick = true;

    /// <summary>
    /// Подобрать
    /// </summary>
    public abstract void Pick();

    /// <summary>
    /// Возможность подбора
    /// </summary>
    /// <param name="gameObject">взаимодействующий персонаж</param>
    public abstract bool CanPick(GameObject gameObject);

    /// <summary>
    /// Уничтожить после поднятия
    /// </summary>
    public virtual void DestroyThisAfterPick()
    {
        Destroy(gameObject);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (CanPick(other.gameObject))
        {
            Pick();

            if (destroyAfterPick)
            {
                DestroyThisAfterPick();
            }
        }
    }
}
