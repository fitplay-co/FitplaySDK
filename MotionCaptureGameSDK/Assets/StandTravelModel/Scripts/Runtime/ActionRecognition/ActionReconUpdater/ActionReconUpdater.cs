using UnityEngine;
using StandTravelModel;

[RequireComponent(typeof(StandTravelModelManager))]
public class ActionReconUpdater : MonoBehaviour
{
    private IActionReconInstance reconInstance;
    private StandTravelModelManager standTravelModelManager;

    private void Awake() {
        reconInstance = CreateReconInstance();
        standTravelModelManager = GetComponent<StandTravelModelManager>();
        this.enabled = standTravelModelManager != null;
    }

    private void Update() {
        if(reconInstance != null && standTravelModelManager)
        {
            reconInstance.OnUpdate(standTravelModelManager.GetKeyPointsList());
        }
    }

    public ActionId GetActionId()
    {
        return reconInstance.GetActionId();
    }

    protected virtual IActionReconInstance CreateReconInstance()
    {
        return new ActionReconInstance();
    }
}