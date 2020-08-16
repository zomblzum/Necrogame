using System;
using System.Collections.Generic;
using UnityEngine;

public class MinionGroupPositioner: MonoBehaviour
{
    [Header("Формация отряда при движении")]
    public GameObject moveFormation;

    private Transform[] movePositionList;

    private void Start()
    {
        movePositionList = moveFormation.GetComponentsInChildren<Transform>();
    }

    public Vector3 GetMovePosition(int minionId)
    {
        // Ставим id+1, потому что метод GetComponentsInChildren получает дочерние объекты + родительский
        // Получается 11 объектов вместо 10
        return movePositionList[minionId + 1].localPosition;
    }
}