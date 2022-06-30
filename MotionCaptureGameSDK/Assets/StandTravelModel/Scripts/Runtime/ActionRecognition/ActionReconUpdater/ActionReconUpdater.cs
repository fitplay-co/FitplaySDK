using UnityEngine;
using StandTravelModel;
using Recorder;

public delegate void OnActionDetect(ActionId actionId);

[RequireComponent(typeof(StandTravelModelManager))]
public partial class ActionReconUpdater : MonoBehaviour
{
    [SerializeField] private bool debug;
    [SerializeField] private bool useRecordData;

    public event OnActionDetect onActionDetect;

    private KeyPointsRecorder keyPointsRecorder;
    private IActionReconInstance reconInstance;
    private StandTravelModelManager standTravelModelManager;

    private void OnValidate() {
        if(reconInstance != null)
        {
            reconInstance.SetDebug(debug);
        }
    }

    private void Awake() {
        this.reconInstance = CreateReconInstance(OnActionDetect);
        this.reconInstance.SetDebug(debug);

        this.standTravelModelManager = GetComponent<StandTravelModelManager>();
        this.enabled = standTravelModelManager != null;

        InitRecorder();
    }

    private void Update() {
        if(reconInstance != null)
        {
            if(useRecordData)
            {
                if(keyPointsRecorder != null)
                {
                    reconInstance.OnUpdate(keyPointsRecorder.GetRecordKeyPoints());
                }
            }
            else
            {
                if(standTravelModelManager != null)
                {
                    reconInstance.OnUpdate(standTravelModelManager.GetKeyPointsList());
                }
            }
        }
    }

    public ActionId GetActionId()
    {
        return reconInstance.GetActionId();
    }

    protected virtual IActionReconInstance CreateReconInstance(OnActionDetect onActionDetect)
    {
        return new ActionReconInstance(onActionDetect);
    }

    private void OnActionDetect(ActionId actionId)
    {
        if(onActionDetect != null)
        {
            onActionDetect(actionId);
        }
    }

    private void InitRecorder()
    {
        if(useRecordData)
        {
            keyPointsRecorder = GetComponent<KeyPointsRecorder>();
            if(keyPointsRecorder == null)
            {
                keyPointsRecorder = gameObject.AddComponent<KeyPointsRecorder>();
            }
        }
    }
}