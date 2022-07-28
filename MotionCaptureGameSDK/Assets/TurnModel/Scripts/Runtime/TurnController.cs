using System;
using MotionCaptureBasic.Interface;
using StandTravelModel.Scripts.Runtime;
using UnityEngine;
using Input = IMU.Input;

namespace TurnModel.Scripts
{
    public class TurnController : MonoBehaviour
    {
        [SerializeField] public StandTravelModelManager standTravelModelManager;

        [Header("A < angle < B时，avatar的转向速率从0开始增加")] [SerializeField] [Range(0, 30)]
        public float A = 5;

        [SerializeField] [Range(0, 90)] public float B = 20;

        [Header("左右转身的A ~ B数据")] [SerializeField] [Range(0, 30)]
        public float A_LR = 5;

        [SerializeField] [Range(0, 90)] public float B_LR = 20;

        [Header("躯干转身的A ~ B数据")] [SerializeField] [Range(0, 30)]
        public float A_FB = 10;

        [SerializeField] [Range(0, 90)] public float B_FB = 60;

        [Header("最大转动速度值")] [SerializeField] [Range(0, 200)]
        public float Wmax = 120;

        [Header("速度曲线设定")] public AnimationCurve SpeedCurve;
        [HideInInspector] public float angle = 0;
        [HideInInspector] public Turn turnLeftOrRight = Turn.None;
        [HideInInspector] public float turnValue = 0;

        [SerializeField] public TurnMode HowToTurn = TurnMode.UseTorsoRotation;
        [SerializeField] public TurnDataType TurnData = TurnDataType.OnlyYshoulder;
        private Vector3 leftShoulder;
        private Vector3 rightShoulder;
        private Vector3 leftHip;
        private Vector3 rightHip;
        private Vector3 centerHip;
        private Vector3 centerShoulder;

        public enum Turn
        {
            None,
            Left,
            Right
        }

        public enum TurnMode
        {
            UseLeftAndRight,
            UseTorsoRotation
        }

        public enum TurnDataType
        {
            OnlyYshoulder,
            OnlyYcrotch,
            BothYshoulderAndYcrotch
        }

        public void SetStandTravelModelManager(StandTravelModelManager comp)
        {
            standTravelModelManager = comp;
        }

        /// <summary>
        /// 模式检测
        /// </summary>
        /// <returns></returns>
        bool IsTraveMode()
        {
            return (standTravelModelManager != null && standTravelModelManager.Enabled &&
                    standTravelModelManager.currentMode == MotionMode.Travel);
        }

        /// <summary>
        /// 根据选择启用检测模式
        /// </summary>
        /// <param name="turn"></param>
        private void ChooseTurnMode(TurnMode turn)
        {
            switch (turn)
            {
                case TurnMode.UseLeftAndRight:
                    TurnByLeftAndRight();
                    break;
                case TurnMode.UseTorsoRotation:
                    TurnByTorso();
                    break;
            }
        }

        /// <summary>
        /// 左右侧身模式
        /// </summary>
        private void TurnByLeftAndRight()
        {
            centerHip = (leftHip + rightHip) / 2;
            centerShoulder = (leftShoulder + rightShoulder) / 2;
            Vector3 targetDir = new Vector3(centerShoulder.x, centerShoulder.y, 0) -
                                new Vector3(centerHip.x, centerHip.y, 0);
            angle = -(Vector3.Angle(targetDir, transform.right) - 90);
            CalcTurnValueByAngle();
        }

        /// <summary>
        /// 躯干转身模式
        /// </summary>
        private void TurnByTorso()
        {
            Vector3 targetDir = Vector3.zero;
            switch (TurnData)
            {
                case TurnDataType.OnlyYshoulder:
                    angle = CalcTurnAngle(rightShoulder, leftShoulder);
                    break;
                case TurnDataType.OnlyYcrotch:
                    angle = CalcTurnAngle(rightHip, leftHip);
                    break;
                case TurnDataType.BothYshoulderAndYcrotch:
                    var angleShoulder = CalcTurnAngle(rightShoulder, leftShoulder);
                    var angleHip = CalcTurnAngle(rightHip, leftHip);
                    angle = Math.Min(angleShoulder, angleHip);
                    break;
            }

            CalcTurnValueByAngle();
        }

        /// <summary>
        /// 计算转动角度
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private float CalcTurnAngle(Vector3 from, Vector3 to)
        {
            var targetDir = new Vector3(to.x, 0, to.z) - new Vector3(from.x, 0, from.z);
            return -(Vector3.Angle(targetDir, transform.forward) - 90);
        }

        /// <summary>
        /// 计算线性转动值
        /// </summary>
        private void CalcTurnValueByAngle()
        {
            if (angle > A)
            {
                //turn right
                turnValue = (angle - A) / (B - A);
                turnValue = SpeedCurve.Evaluate(turnValue);
                turnLeftOrRight = Turn.Right;
                Input.MCTurnValue = turnValue;
            }
            else if (angle < -A)
            {
                //turn left
                turnValue = -(angle + A) / (B - A);
                turnValue = -SpeedCurve.Evaluate(turnValue);
                turnLeftOrRight = Turn.Left;
                Input.MCTurnValue = turnValue;
            }
            else
            {
                //no turn
                turnLeftOrRight = Turn.None;
                turnValue = 0;
                Input.MCTurnValue = 0;
            }
        }

        void Update()
        {
            if (IsTraveMode())
            {
                var keyPointList = standTravelModelManager.GetKeyPointsList();
                if (keyPointList == null) return;
                leftShoulder = keyPointList[(int) GameKeyPointsType.LeftShoulder];
                rightShoulder = keyPointList[(int) GameKeyPointsType.RightShoulder];
                leftHip = keyPointList[(int) GameKeyPointsType.LeftHip];
                rightHip = keyPointList[(int) GameKeyPointsType.RightHip];
                //覆盖原来参数
                standTravelModelManager.tuningParameters.RotationSensitivity = Wmax;
                ChooseTurnMode(HowToTurn);
            }
        }
    }
}