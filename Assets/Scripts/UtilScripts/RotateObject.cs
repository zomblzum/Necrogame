using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Поворачивающийся объект
/// </summary>
public class RotateObject : MonoBehaviour
{
    [Header("Скорость поворота по осям")]
    public Vector3 rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateSpeed.x * Time.deltaTime, 
            rotateSpeed.y * Time.deltaTime, 
            rotateSpeed.z * Time.deltaTime, 
            Space.Self);
    }
}
