public class ZombieGroupZontroller : MinionGroupController
{
    public MinionBehaviour minionBehaviour;

    public override void MinionAdded(Minion minion)
    {
        base.MinionAdded(minion);
        GoOutConrolIfAboveMaxMinions();
    }

    public override void MinionRemoved()
    {
        GoUnderConrolIfNotMaxMinions();
    }

    public void GoOutConrolIfAboveMaxMinions()
    {
        if (minionBehaviour.minionGroups[0].minions.Count > minionBehaviour.maxMinions)
        {
            SetGroupOutControl();
        }
    }

    public void GoUnderConrolIfNotMaxMinions()
    {
        if (minionBehaviour.minionGroups[0].minions.Count <= minionBehaviour.maxMinions)
        {
            SetGroupUnderControl();
        }
    }
}
