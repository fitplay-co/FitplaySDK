using UnityEngine;
using StandTravelModel;
using Recorder;
using MotionCaptureBasic;

public delegate void OnActionDetect(ActionId actionId);

[RequireComponent(typeof(StandTravelModelManager))]
public partial class ActionReconUpdater : MonoBehaviour
{
    protected enum ReconState
    {
        None,
        Simulat,
        Fake
    }

    [SerializeField] private bool debug;
    [SerializeField] private bool useRecordData;
    [SerializeField] protected ReconState reconState;

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
        //this.enabled = standTravelModelManager != null;

        InitRecorder();
    }

    protected virtual void Update() {
        if(reconInstance != null)
        {
            if(reconState != ReconState.None)
            {
                if(useRecordData)
                {
                    MotionDataModelHttp.GetInstance().SetIKDataListSimulat(keyPointsRecorder.GetRecordKeyPoints());
                }

                if(reconState == ReconState.Simulat)
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
        if(reconState != ReconState.None)
        {
            keyPointsRecorder = GetComponent<KeyPointsRecorder>();
            if(keyPointsRecorder == null)
            {
                keyPointsRecorder = gameObject.AddComponent<KeyPointsRecorder>();
            }
        }
    }
}