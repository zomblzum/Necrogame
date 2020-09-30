using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickable : PickableObject
{
    [Header("Восстанавливаемое здоровье")]
    public int health = 10;

    public override bool CanPick(GameObject gameObject)
    {
        if(gameObject.GetComponent<Player>())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Pick()
    {
        FindObjectOfType<Player>().AddHealth(health);
    }
}
