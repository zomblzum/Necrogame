using UnityEngine;

public class GoldPickable : PickableObject
{
    [Header("Количество денег, которые получит игрок")]
    public int money = 10;
    
    public override bool CanPick(GameObject gameObject)
    {
        if(gameObject.GetComponent<Player>() || (gameObject.GetComponent<Minion>() && gameObject.GetComponent<Minion>().underControl))
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
        FindObjectOfType<Player>().AddMoney(money);
    }
}
