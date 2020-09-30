using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPickable : PickableObject
{
    [Header("Восстанавливаемая мана")]
    public int mana = 10;

    public override bool CanPick(GameObject gameObject)
    {
        if (gameObject.GetComponent<Player>())
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
        FindObjectOfType<Player>().AddMana(mana);
    }
}
