using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Прицел, необходимый для отслеживания персонажа для атаки
/// </summary>
public class PlayerTarget : MonoBehaviour
{
    private GameObject currentTarget;

    public GameObject GetCurrentTarget()
    {
        return currentTarget;
    }

    public Vector3 GetCurrentPosition()
    {
        return transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<IAttackable>() != null)
        {
            currentTarget = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(currentTarget == other.gameObject)
        {
            currentTarget = null;
        }
    }
}
