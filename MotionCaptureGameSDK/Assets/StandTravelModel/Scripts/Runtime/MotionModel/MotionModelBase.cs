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
        protected float groundHeight;

        private Vector3 predictHipPos = Vector3.zero;
        private Transform keyPointsParent;
        private int layerMask;

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

            var layerIndex = LayerMask.NameToLayer("Ground");
            if (layerIndex == -1)
            {
                layerMask = Physics.DefaultRaycastLayers;
            }
            else
            {
                layerMask = 1 << layerIndex;
            }
        }

        public virtual void OnFixedUpdate()
        {
            var startPos = selfTransform.position + new Vector3(0, 5, 0);

            if (Physics.Raycast(startPos, Vector3.down, out var hit, 100, layerMask))
            {
                groundHeight = hit.point.y;
                //Debug.LogError($"Ray Cast Ground Height: {groundHeight}");
            }
            else
            {
                groundHeight = anchorController.StandFollowPoint.transform.position.y;
            }
        }

        public virtual void OnUpdate(List<Vector3> keyPoints)
        {
            PrepareData();
        }

        public virtual void OnLateUpdate()
        {
            //利用打线获取到的地面高度修正两个锚点的y
            var standPos = anchorController.StandFollowPoint.transform.position;
            var travelPos = anchorController.TravelFollowPoint.transform.position;

            standPos.y = groundHeight;
            travelPos.y = groundHeight;

            anchorController.StandFollowPoint.transform.position = standPos;
            anchorController.TravelFollowPoint.transform.position = travelPos;

            selfTransform.position += Vector3.Scale(predictHipPos, tuningParameters.ScaleMotionPos) +
                                      tuningParameters.HipPosOffset - characterHipNode.position +
                                      travelPos;
        }

        public virtual void Clear()
        {
            anchorController?.DestroyObject();
            anchorController = null;
        }

        /// <summary>
        /// 利用ground location计算相对镜头的位移，实现stand模式的小范围移动。如果打开宏开关，则使用客户端自行计算的方式来获取偏移
        /// </summary>
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
            
            //Debug.Log($"Local Shift: {shiftX}, Hip Height: {predictHipPos.y}");
#else
            var groundLocationData = motionDataModel.GetGroundLocationData();
            //Debug.LogError($"Ground Location: x = {groundLocationData.x}, y = {groundLocationData.y}, z = {groundLocationData.z}");

            predictHipPos.y = groundLocationData.y * tuningParameters.LocalShiftScale.y;
            keyPointsParent.transform.localPosition = predictHipPos;
            var planeShift = new Vector3(-groundLocationData.x * tuningParameters.LocalShiftScale.x, 0,
                -groundLocationData.z * tuningParameters.LocalShiftScale.z);
#endif
            localShift = anchorController.TravelFollowPoint.transform.rotation * planeShift;
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

        public float GetGroundHeight()
        {
            return groundHeight;
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