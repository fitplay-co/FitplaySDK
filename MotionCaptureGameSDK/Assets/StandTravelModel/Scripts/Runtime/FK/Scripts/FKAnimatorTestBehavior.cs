using System;
using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Scripts
{
    public class FKAnimatorTestBehavior : MonoBehaviour
    {
        private IMotionDataModel motionDataModel;
        private bool _osConnected = false;
        public bool osConnected => _osConnected;
        public FKAnimatorJoints fkSolver = null;
        public FKAnimatorBasedLocomotion locomotion = null;
        public bool isDebug = false;

        public void Awake() {
            InitMotionDataModel();
        }

        // Start is called before the first frame update
        void Start() {
            motionDataModel.SetDebug(isDebug);
        }

        // Update is called once per frame
        void Update() {
            Fitting fittingFrame = motionDataModel.GetFitting();
            if(fittingFrame != null) {
                if(fkSolver != null) {
                    fkSolver.UpdateFkInfo(fittingFrame);
                }
                if(locomotion != null) {
                    locomotion.updateGroundLocationHint(motionDataModel);
                }
            }
        }

        private void LateUpdate()
        {
            if (locomotion != null)
            {
                locomotion.UpdateLocomotion();
            }
        }

        public void ResetGroundLocation() {
            motionDataModel.ResetGroundLocation();
        }

        private void InitMotionDataModel() {
            motionDataModel = MotionDataModelFactory.Create(MotionDataModelType.Http);
            motionDataModel.AddConnectEvent(SubscribeMessage);
        }

        private void SubscribeMessage() {
            motionDataModel.SubscribeActionDetection();
            motionDataModel.SubscribeGroundLocation();
            motionDataModel.SubscribeFitting();
            _osConnected = true;
        }
    }
}
