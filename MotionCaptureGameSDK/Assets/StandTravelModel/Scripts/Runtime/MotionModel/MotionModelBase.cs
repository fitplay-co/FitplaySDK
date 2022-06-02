using System.Collections.Generic;
using UnityEngine;
using MotionCaptureBasic.Interface;

namespace StandTravelModel.Core
{
    public abstract class MotionModelBase : IMotionModel
    {
        protected Vector3 localShift;
        protected Transform selfTransform;
        protected Transform characterHipNode;
        protected TuningParameterGroup tuningParameters;
        protected StandTravelAnchorController anchorController;

        private Vector3 predictHipPos;
        private Transform keyPointsParent;
        private IMotionDataModel motionDataModel;

        public MotionModelBase(Transform selfTransform, Transform characterHipNode, Transform keyPointsParent, TuningParameterGroup tuningParameters, IMotionDataModel motionDataModel, StandTravelAnchorController anchorController)
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
    }
}