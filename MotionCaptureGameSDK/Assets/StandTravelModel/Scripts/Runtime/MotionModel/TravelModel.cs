using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MotionCaptureBasic.FSM;
using MotionCaptureBasic.Interface;
using StandTravelModel.Core;
using StandTravelModel.Core.AnimationStates;
using UnityEngine;

namespace StandTravelModel.MotionModel
{
    public class TravelModel : MotionModelBase
    {
        public int currentLeg = 0;
        public float currentFrequency = 0;
        public bool isJump = false;

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
        private const int CacheQueueMax = 11;
        //判断抬腿CacheQueueMax次总时长是否进入跑步模式的阈值
        private const float StepMaxInterval = 5;

        //private TravelModelAnimatorController animatorController;

        public TravelModel(
            Transform selfTransform,
            Transform characterHipNode,
            Transform keyPointsParent,
            TuningParameterGroup tuningParameters,
            IMotionDataModel motionDataModel,
            AnchorController anchorController,
            AnimatorSettingGroup animatorSettingGroup
            ) : base(
                selfTransform,
                characterHipNode,
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

            animationStates = new Dictionary<AnimationList, State<MotionModelBase>>
            {
                {AnimationList.Idle, new TravelIdleState(this)},
                {AnimationList.Run, new TravelRunState(this)},
                {AnimationList.Jump, new TravelJumpState(this)},
                {AnimationList.LeftStep, new TravelLeftStepState(this)},
                {AnimationList.RightStep, new TravelRightStepState(this)},
                {AnimationList.Squat, new TravelSquatState(this)}
            };
            
            stateMachine = new StateMachine<MotionModelBase>(animationStates[AnimationList.Idle]);
        }

        public override void OnLateUpdate()
        {
            anchorController.StandFollowPoint.transform.position =
                        anchorController.TravelFollowPoint.transform.position - localShift;
                    selfTransform.rotation = anchorController.TravelFollowPoint.transform.rotation;

            base.OnLateUpdate();
        }

        public override void OnUpdate(List<Vector3> keyPoints)
        {
            base.OnUpdate(keyPoints);
            //animatorController.UpdateTravelAnimator();

            var deltaTime = Time.deltaTime;

            stateMachine.OnTick(deltaTime);
        }

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
                    return;
                }
            }
            
            stepCacheQueue.Add(new StepStct{LegUp = leg, TimeStemp = Time.time});

            length = stepCacheQueue.Count;
            if (length > CacheQueueMax)
            {
                stepCacheQueue.RemoveAt(0);
            }
        }

        /// <summary>
        /// 利用11次交互抬腿的队列记录，计算11次抬腿的总时长
        /// 小于StepMaxInterval定义的时长才进入跑步模式(返回true)，否则为单步模式(返回false)
        /// </summary>
        /// <returns></returns>
        public bool IsEnterRunReady()
        {
            var length = stepCacheQueue.Count;
            if (length < CacheQueueMax)
            {
                return false;
            }

            var interval = stepCacheQueue[length - 1].TimeStemp - stepCacheQueue[0].TimeStemp;
            if (interval < StepMaxInterval)
            {
                return true;
            }

            return false;
        }

        public void UpdateAnimatorCadence()
        {
            selfAnimator.SetFloat(Cadence, currentFrequency);
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
    }

    public struct StepStct
    {
        public int LegUp;
        public float TimeStemp;
    }
}