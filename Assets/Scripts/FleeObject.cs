using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Объек, способный убегать от кого либо
/// </summary>
public abstract class FleeObject : MonoBehaviour
{
    [SerializeField]
    [Header("Зона защиты")]
    private CapsuleCollider areaCollider;

    protected List<GameObject> targets;
    protected GameObject mainTarget;

    public virtual void Awake()
    {
        targets = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!targets.Contains(other.gameObject) && CorrectTarget(other.gameObject))
        {
            targets.Add(other.gameObject);

            if (!mainTarget)
            {
                mainTarget = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targets.Contains(other.gameObject) && CorrectTarget(other.gameObject))
        {
            targets.Remove(other.gameObject);
            targets.RemoveAll(item => item == null);

            if (mainTarget == other.gameObject)
            {
                mainTarget = null;
            }

            if (!mainTarget && TargetsCount() > 0)
            {
                mainTarget = targets[0];
            }
        }
    }

    /// <summary>
    /// Установить радиус зоны
    /// </summary>
    /// <param name="radius"></param>
    public void SetAreaRadius(float radius)
    {
        areaCollider.radius = radius;
    }

    /// <summary>
    /// Рассчитать координаты для побега
    /// </summary>
    /// <returns>вектор с координатами</returns>
    public virtual Vector3 GetFleeCoordinates()
    {
        if (mainTarget)
        {
            return transform.position + (transform.position - mainTarget.transform.position);
        }
        else
        {
            return transform.position;
        }
    }

    /// <summary>
    /// Проверка на подходящую цель
    /// </summary>
    /// <param name="target">вошедший в зону обнаружения враг</param>
    public abstract bool CorrectTarget(GameObject target);

    /// <summary>
    /// Количество вражеских целей
    /// </summary>
    public virtual int TargetsCount()
    {
        return targets.Count;
    }
}
