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

    private void OnDrawGizmos()
    {
        //Рисуем сферу
        Gizmos.color = new Color(0, 0, 0);
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one) * Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        Gizmos.DrawWireSphere(Vector3.zero, 3f);
        //Рисуем стрелку
        Gizmos.color = new Color(1, 0, 0);
        DrawArrow.ForGizmo(new Vector3(3f, 0, 0), (Quaternion.Euler(-90,0,0) * rotateSpeed)/100);
    }
}
