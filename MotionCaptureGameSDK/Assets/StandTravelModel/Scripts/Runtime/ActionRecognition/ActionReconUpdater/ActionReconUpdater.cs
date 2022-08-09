using System.Collections.Generic;
using MotionCaptureBasic;
using MotionCaptureBasic.OSConnector;
using StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconInstance;
using StandTravelModel.Scripts.Runtime.ActionRecognition.Recorder;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconUpdater
{
    public delegate void OnActionDetect(ActionId actionId);

    [RequireComponent(typeof(StandTravelModelManager))]
    public partial class ActionReconUpdater : MonoBehaviour
    {
        protected enum ReconState
        {
            None,
            Simulat,
            Fake,
            FromFile
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
                        List<Vector3> keyPoints = null;
                        ActionDetectionItem actionDetectionItem = null;
                        keyPointsRecorder.GetRecordDatas(out keyPoints, out actionDetectionItem);

                        MotionDataModelHttp.GetInstance().SetIKDataListSimulat(keyPoints);
                        MotionDataModelHttp.GetInstance().SetSimulatActionDetectionData(actionDetectionItem);
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
            return new ActionReconInstance.ActionReconInstance(onActionDetect);
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
}