using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Minion : Character
{
    [Header("Подчиняется игроку в данный момент")]
    public bool underControl = true;
    [Header("Теги персонажей, на которые может осуществлятся нападение")]
    public List<string> targetsTags;

    protected MinionBehaviour minionBehaviour;
    protected Player player;

    protected virtual void Awake()
    {
        player = FindObjectOfType<Player>();
        minionBehaviour = FindObjectOfType<MinionBehaviour>();
        attackTargets = new List<GameObject>();
    }

    protected override void CharacterBehaviour()
    {
        base.CharacterBehaviour();

        if (attackTarget == null)
        {
            FindTargetsByTags();
            attackTargets.RemoveAll(item => item == null);
            if (attackTargets.Count > 0)
            {
                inAggro = true;
                GetClosestTarget();
            }
            else
            {
                inAggro = false;
            }
        }
    }

    public override void Die()
    {
        minionBehaviour.RemoveMinion(this);

        base.Die();
    }

    /// <summary>
    /// Выбираем текущий список цели по тегам врагом
    /// </summary>
    public virtual void FindTargetsByTags()
    {
        attackTargets.Clear();
        foreach(string tag in targetsTags)
        {
            attackTargets.AddRange(GameObject.FindGameObjectsWithTag(tag));
        }
        attackTargets.Remove(this.gameObject);
    }

    /// <summary>
    /// Выход из под контроля
    /// </summary>
    public abstract void GoOutControl();

    /// <summary>
    /// Возвращение под контроль
    /// </summary>
    public abstract void GoUnderControl();
}
