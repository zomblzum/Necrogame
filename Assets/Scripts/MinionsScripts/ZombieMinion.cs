using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ZombieMinion : Minion
{
    private void OnTriggerEnter(Collider other)
    {
        Corpse corpse = other.gameObject.GetComponent<Corpse>();
        if (corpse != null && !corpse.ResurectInProcess())
        {
            corpse.Resurect(minionBehaviour);
        }
    }

    public override void Die()
    {
        FindObjectOfType<MinionBehaviour>().RemoveMinion(this);
        Instantiate(deathEffect, transform.position, Quaternion.Euler(new Vector3(0f, 0f)));
        Destroy(this.gameObject);
    }

    public override void GoOutControl()
    {
        underControl = false;
        targetsTags.Add("Minion");
        targetsTags.Add("Player");
        FindTargetsByTags();
        GetClosestTarget();
    }

    public override void GoUnderControl()
    {
        underControl = true;
        targetsTags.Remove("Minion");
        targetsTags.Remove("Player");
        FindTargetsByTags();
        GetClosestTarget();
    }
}
