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

        private void PrepareData()
        {
            var groundLocationData = motionDataModel.GetGroundLocationData();
            //Debug.Log($"Ground Location: x = {groundLocationData.x}, y = {groundLocationData.y}, z = {groundLocationData.z}");

            predictHipPos.y = groundLocationData.y * tuningParameters.LocalShiftScale.y;
            keyPointsParent.transform.localPosition = predictHipPos;

            var planeShift = new Vector3(-groundLocationData.x * tuningParameters.LocalShiftScale.x, 0,
                -groundLocationData.z * tuningParameters.LocalShiftScale.z);

            localShift = anchorController.TravelFollowPoint.transform.rotation * planeShift;
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