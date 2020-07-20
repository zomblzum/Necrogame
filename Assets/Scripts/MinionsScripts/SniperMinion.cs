
using UnityEngine;

/// <summary>
/// Разбойник
/// Требует деньги с игрока по прошествии какого то времени
/// </summary>
public class SniperMinion : Minion
{
    private SniperMinionInteractable banditPayInteractable;

    protected override void Awake()
    {
        base.Awake();
        banditPayInteractable = GetComponent<SniperMinionInteractable>();
    }

    public override void GoOutControl()
    {
        underControl = false;
        inAggro = false;
        banditPayInteractable.SetActiveStatus(true);
    }

    public override void GoUnderControl()
    {
        underControl = true;
        inAggro = true;
        banditPayInteractable.SetActiveStatus(false);
    }

    protected override void AttackTargetInteractionsBehaviour()
    {
        if (underControl)
        {
            base.AttackTargetInteractionsBehaviour();
        }
    }
}
