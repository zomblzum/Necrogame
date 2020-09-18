using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Реализация крестьянина
/// </summary>
public class Villager : Character
{
    private FleeObject fleeObject;

    void Awake()
    {
        fleeObject = GetComponent<FleeObject>();
    }

    protected override void CharacterBehaviour()
    {
        if (fleeObject.TargetsCount() > 0)
        {
            SetMovePoint(fleeObject.GetFleeCoordinates());
        } 
        else if (movePoint == Vector3.zero)
        {
            PassiveBehaviour();
        }

        if (movePoint != Vector3.zero)
        {
            MovingBehaviour();
        }
    }
}
