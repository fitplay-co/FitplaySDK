using System;
using System.Collections.Generic;
using MotionCaptureBasic.FSM;
using MotionCaptureBasic.Interface;
using StandTravelModel.Scripts.Runtime.Core;
using StandTravelModel.Scripts.Runtime.Core.AnimationStates;
using StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.MotionModel
{
    public class TravelModel : MotionModelBase
    {
        public int currentLeg = 0;
        public float currentFrequency = 0;
        public bool isJump = false;
        public bool isRun = false;

        private AnimatorSettingGroup _animatorSettings;
        public AnimatorSettingGroup animatorSettings => _animatorSettings;

        private Animator _selfAnimator;
        public Animator selfAnimator => _selfAnimator;

        private Dictionary<AnimationList, AnimatorParamStct> _animSettingMap;
        public Dictionary<AnimationList, AnimatorParamStct> animSettingMap => _animSettingMap;

        public IMotionDataModel selfMotionDataModel => motionDataModel;

        public List<StepStct> stepCacheQueue;
        private static readonly int Cadence = Animator.StringToHash("cadence");

        //抬腿交互缓存队列的总长度
        private int _cacheQueueMax = 11;
        //private StriderBiped striderBiped;

        public int cacheQueueMax
        {
            set => _cacheQueueMax = value;
            get => _cacheQueueMax;
        }

        //判断抬腿CacheQueueMax次总时长是否进入跑步模式的阈值
        private float _stepMaxInterval = 5;

        public float stepMaxInterval
        {
            set => _stepMaxInterval = value;
            get => _stepMaxInterval;
        }

        private Vector3 _moveVelocity;
        public Vector3 moveVelocity => _moveVelocity;

        private bool isExControlMode;
        private RunConditioner runConditioner;

        //private TravelModelAnimatorController animatorController;

        public TravelModel(
            Transform selfTransform,
            Transform characterHipNode,
            Transform characterHeadNode,
            Transform keyPointsParent,
            TuningParameterGroup tuningParameters,
            IMotionDataModel motionDataModel,
            AnchorController anchorController,
            AnimatorSettingGroup animatorSettingGroup,
            bool isExControl,
            AnimationCurve speedCurve,
            AnimationCurve downCurve,
            StepStateSmoother stepSmoother,
            Func<float> getRunThrehold,
            Func<float> strideScale,
            Func<float> strideScaleRun,
            Func<bool> useFrequency
        ) : base(
            selfTransform,
            characterHipNode,
            characterHeadNode,
            keyPointsParent,
            tuningParameters,
            motionDataModel,
            anchorController
        )
        {
            /*animatorController = new TravelModelAnimatorController(selfTransform.GetComponent<Animator>(),
                motionDataModel, anchorController, animatorSettingGroup);*/
            stepCacheQueue = new List<StepStct>();

            _animatorSettings = animatorSettingGroup;

            _animSettingMap = new Dictionary<AnimationList, AnimatorParamStct>();

            foreach (var item in _animatorSettings.animationMap)
            {
                _animSettingMap[item.animType] = item.animParam;
            }

            _selfAnimator = selfTransform.GetComponent<Animator>();

            var strideCacher = new StepStrideCacher();
            this.runConditioner = new RunConditioner(getRunThrehold, useFrequency, strideCacher);
            var parametersSetter = new StepStateAnimatorParametersSetter(this, speedCurve, downCurve, stepSmoother, strideCacher, strideScale, strideScaleRun);

            animationStates = new Dictionary<AnimationList, State<MotionModelBase>>
            {
                {AnimationList.Idle, new TravelIdleState(this, parametersSetter, runConditioner)},
                {AnimationList.Run, new TravelRunState(this, parametersSetter, runConditioner)},
                {AnimationList.Jump, new TravelJumpState(this)},
                {AnimationList.LeftStep, new TravelLeftStepState(this, parametersSetter, runConditioner)},
                {AnimationList.RightStep, new TravelRightStepState(this, parametersSetter, runConditioner)},
                {AnimationList.Squat, new TravelSquatState(this)}
            };
            
            stateMachine = new StateMachine<MotionModelBase>(animationStates[AnimationList.Idle]);

            cacheQueueMax = tuningParameters.CacheStepCount;
            stepMaxInterval = tuningParameters.StepToRunTimeThreshold;

            isExControlMode = isExControl;
        }

        public override void OnLateUpdate()
        {
            anchorController.StandFollowPoint.transform.position =
                anchorController.TravelFollowPoint.transform.position - localShift;
            selfTransform.rotation = anchorController.TravelFollowPoint.transform.rotation;

            if (isExControlMode)
            {
                var newPos = selfTransform.position;
                newPos.y = groundHeight;
                anchorController.TravelFollowPoint.transform.position = newPos;
            }
            else
            {
                base.OnLateUpdate();
            }
        }

        public void FixAvatarHeight()
        {
            if (isExControlMode)
            {
                var newPos = selfTransform.position;
                newPos.y = groundHeight + 0.1f;
                selfTransform.position = newPos;
            }
        }

        public override void OnUpdate(List<Vector3> keyPoints)
        {
            base.OnUpdate(keyPoints);
            //animatorController.UpdateTravelAnimator();

            var deltaTime = Time.deltaTime;

            stateMachine.OnTick(deltaTime);

            if (!isExControlMode)
            {
                anchorController.MoveTravelPoint(_moveVelocity * deltaTime);
            }
        }
		
		/* public override void UpdateFromMono()
        {
            stateMachine.OnTick(Time.deltaTime);
        } */

        /// <summary>
        /// 记录11次抬腿。抬腿交错一次才记录一次到队列。总共左右交互记录11次
        /// </summary>
        /// <param name="leg"></param>
        public void EnqueueStep(int leg)
        {
            if (leg == 0)
            {
                return;
            }

            var length = stepCacheQueue.Count;
            if (length > 0)
            {
                var stepStct = stepCacheQueue[length - 1];
                if (stepStct.LegUp == leg)
                {
                    stepStct.TimeStemp = Time.time;
                    stepCacheQueue[length - 1] = stepStct;
                    return;
                }
            }

            stepCacheQueue.Add(new StepStct {LegUp = leg, TimeStemp = Time.time});

            length = stepCacheQueue.Count;
            if (length > _cacheQueueMax)
            {
                stepCacheQueue.RemoveAt(0);
            }
        }

        /// <summary>
        /// 利用cacheQueueMax次交互抬腿的队列记录，计算cacheQueueMax次抬腿的总时长
        /// 小于stepMaxInterval定义的时长才进入跑步模式(返回true)，否则为单步模式(返回false)
        /// </summary>
        /// <returns></returns>
        public bool IsEnterRunReady()
        {
            var length = stepCacheQueue.Count;
            if (length < _cacheQueueMax)
            {
                return false;
            }

            var interval = stepCacheQueue[length - 1].TimeStemp - stepCacheQueue[0].TimeStemp;
            //Debug.LogError($"step cache interval： {interval}");
            if (interval < _stepMaxInterval)
            {
                return true;
            }

            return false;
        }

        public void UpdateAnimatorCadence()
        {
            selfAnimator.SetFloat(Cadence, currentFrequency);
        }

        public void UpdateVelocity(Vector3 v)
        {
            //Debug.LogError($"Current Velocity: {v}");
            _moveVelocity = v;
        }

        /*public void StopPrevAnimation(string currentState)
        {
            animatorController.StopPrevAnimation(currentState);
        }*/

        /*public override void Clear()
        {
            base.Clear();
            animatorController.Clear();
            animatorController = null;
        }*/

        public RunConditioner GetRunConditioner()
        {
            return runConditioner;
        }
    }

    public struct StepStct
    {
        public int LegUp;
        public float TimeStemp;
    }
}