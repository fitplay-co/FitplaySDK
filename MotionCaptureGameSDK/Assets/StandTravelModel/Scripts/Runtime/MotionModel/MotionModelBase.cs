using System.Collections.Generic;
using MotionCaptureBasic.FSM;
using MotionCaptureBasic.Interface;
using StandTravelModel.Core;
using UnityEngine;

namespace StandTravelModel.MotionModel
{
    public abstract class MotionModelBase : IMotionModel
    {
        protected Vector3 localShift;
        protected Transform selfTransform;
        protected Transform characterHipNode;
        protected AnchorController anchorController;
        protected TuningParameterGroup tuningParameters;

        private Vector3 predictHipPos;
        private Transform keyPointsParent;
        private List<Vector3> keyPoints;

        protected IMotionDataModel motionDataModel;
        protected StateMachine<MotionModelBase> stateMachine;
        protected Dictionary<AnimationList, State<MotionModelBase>> animationStates;

        public MotionModelBase(Transform selfTransform, Transform characterHipNode, Transform keyPointsParent,
            TuningParameterGroup tuningParameters, IMotionDataModel motionDataModel, AnchorController anchorController)
        {
            this.selfTransform = selfTransform;
            this.keyPointsParent = keyPointsParent;
            this.motionDataModel = motionDataModel;
            this.tuningParameters = tuningParameters;
            this.characterHipNode = characterHipNode;
            this.anchorController = anchorController;
        }

        public virtual void OnUpdate(List<Vector3> keyPoints)
        {
            this.keyPoints = keyPoints;
            PrepareData();
        }

        public virtual void OnLateUpdate()
        {
            selfTransform.position += Vector3.Scale(predictHipPos, tuningParameters.ScaleMotionPos) +
                                      tuningParameters.HipPosOffset - characterHipNode.position +
                                      anchorController.TravelFollowPoint.transform.position;
        }

        public virtual void Clear()
        {
            anchorController?.DestroyObject();
            anchorController = null;
        }

        public List<Vector3> GetKeyPoints()
        {
            return keyPoints;
        }

        private void PrepareData()
        {
#if NOT_USE_GROUND_LOCATION
            predictHipPos.y = (0 - GetMinY(motionDataModel.GetIKPointsData(true, true))) *
                              tuningParameters.LocalShiftScale.y;
            keyPointsParent.transform.localPosition = predictHipPos;
            
            var keyPoints = motionDataModel.GetIKPointsData(false, true);
            var shiftX = (keyPoints[(int) GameKeyPointsType.LeftHip].x + keyPoints[(int) GameKeyPointsType.RightHip].x) /
                2 - 0.5f;
            var planeShift = new Vector3(shiftX * tuningParameters.LocalShiftScale.x, 0, 0);
            localShift = anchorController.TravelFollowPoint.transform.rotation * planeShift;
            
            //Debug.Log($"Local Shift: {shiftX}, Hip Height: {predictHipPos.y}");
#else
            var planeShift = Vector3.zero;
            var groundLocationData = motionDataModel.GetGroundLocationData();
            //Debug.Log($"Ground Location: x = {groundLocationData.x}, y = {groundLocationData.y}, z = {groundLocationData.z}");
            if(groundLocationData != null)
            {
                predictHipPos.y = groundLocationData.y * tuningParameters.LocalShiftScale.y;
                keyPointsParent.transform.localPosition = predictHipPos;
                planeShift = new Vector3(-groundLocationData.x * tuningParameters.LocalShiftScale.x, 0,
                    -groundLocationData.z * tuningParameters.LocalShiftScale.z);

                localShift = anchorController.TravelFollowPoint.transform.rotation * planeShift;
            }
#endif
        }

        private float GetMinY(List<Vector3> keyPoints3D)
        {
            float minY = 1;
            foreach (var point in keyPoints3D)
            {
                var y = point.y;
                if (y < minY)
                {
                    minY = y;
                }
            }

            return minY;
        }

        public AnchorController GetAnchorController()
        {
            return anchorController;
        }

        public void ChangeState(AnimationList animationState)
        {
            if (animationStates == null) return;
            if (animationStates.TryGetValue(animationState, out var nextState))
            {
                stateMachine.ChangeState(nextState);
            }
        }

        public void ChangePrevState()
        {
            if (stateMachine == null) return;
            stateMachine.ChangePrevState();
        }

        public State<MotionModelBase> GetCurrentState()
        {
            return stateMachine.CurrentState;
        }

        public State<MotionModelBase> GetPrevState()
        {
            return stateMachine.PrevState;
        }
    }
}