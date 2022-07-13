using UnityEngine;

public class ActionReconUpdaterDragon : ActionReconUpdater
{
    protected override IActionReconInstance CreateReconInstance(OnActionDetect onActionDetect)
    {
        return new ActionReconInstanceDragon(onActionDetect);
    }
}