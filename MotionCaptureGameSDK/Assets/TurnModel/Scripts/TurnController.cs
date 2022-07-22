using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using StandTravelModel.Scripts.Runtime;
using UnityEngine;
using Input = IMU.Input;
namespace TurnModel.Scripts
{
    public class TurnController : MonoBehaviour
    {
        [SerializeField] private StandTravelModelManager standTravelModelManager;
        [SerializeField] private LineRenderer hipLineRenderer;
        [SerializeField] private LineRenderer shoulderLineRenderer;
        [SerializeField] private LineRenderer angleLineRenderer;
        [SerializeField] private Transform shoulderTarget;
        [SerializeField] private Transform hipTarget;

        [Header("A < angle < B时，avatar的转向速率从0开始增加")] [SerializeField] [Range(0, 30)]
        public float A = 5;

        [SerializeField] [Range(0, 90)] public float B = 20;

        //[Header("当前X减去上一秒X的差值(C)小于0")]
        //[SerializeField] [Range(0, 90)] public float c = 5;
        [Header("最大转动速度值")] [SerializeField] [Range(0, 200)]
        public float Wmax = 120;

        [Header("速度曲线设定")] public AnimationCurve SpeedCurve;
        [HideInInspector] public float angle = 0;
        [HideInInspector] public Turn turnLeftOrRight = Turn.None;
        [HideInInspector] public float turnValue = 0;

        private List<Vector3> keyPointList;
        private bool showDebug = false;

        public enum Turn
        {
            None,
            Left,
            Right
        }

        /// <summary>
        /// Debug使用，还需要完善
        /// </summary>
        /// <param name="lineRenderer"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        void DrawLine(LineRenderer lineRenderer, Vector3 from, Vector3 to)
        {
            if (lineRenderer == null) return;
            lineRenderer.SetPosition(0, from);
            lineRenderer.SetPosition(1, to);
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

        void Update()
        {
            if (IsTraveMode())
            {
                var keyPointList = standTravelModelManager.GetKeyPointsList();
                if (keyPointList == null) return;
                var leftShoulder = keyPointList[(int) GameKeyPointsType.LeftShoulder];
                var rightShoulder = keyPointList[(int) GameKeyPointsType.RightShoulder];
                var leftHip = keyPointList[(int) GameKeyPointsType.LeftHip];
                var rightHip = keyPointList[(int) GameKeyPointsType.RightHip];
                var centerHip = (leftHip + rightHip) / 2;
                var centerShoulder = (leftShoulder + rightShoulder) / 2;
                hipTarget.localPosition = centerHip;
                shoulderTarget.localPosition = centerShoulder;
                if (showDebug)
                {
                    transform.position = standTravelModelManager.transform.position + new Vector3(0, 1.1f, -0.3f);
                    hipTarget.gameObject.SetActive(true);
                    shoulderTarget.gameObject.SetActive(true);
                    DrawLine(hipLineRenderer, leftHip, rightHip);
                    DrawLine(shoulderLineRenderer, leftShoulder, rightShoulder);
                    DrawLine(angleLineRenderer, centerHip, centerShoulder);
                }
                else
                {
                    hipTarget.gameObject.SetActive(false);
                    shoulderTarget.gameObject.SetActive(false);
                }

                //覆盖原来参数
                standTravelModelManager.tuningParameters.RotationSensitivity = Wmax;
                Vector3 targetDir = new Vector3(shoulderTarget.position.x, shoulderTarget.position.y, 0) -
                                    new Vector3(hipTarget.position.x, hipTarget.position.y, 0);
                angle = Vector3.Angle(targetDir, transform.up);
                if (angle > A)
                {
                    turnValue = (angle - A) / (B - A);
                    turnValue = SpeedCurve.Evaluate(turnValue);
                    if (shoulderTarget.position.x > hipTarget.position.x)
                        turnLeftOrRight = Turn.Right;
                    else
                    {
                        turnLeftOrRight = Turn.Left;
                        turnValue = -turnValue;
                    }

                    Input.SpineTurnValue = turnValue;
                }
                else
                {
                    turnLeftOrRight = Turn.None;
                    turnValue = 0;
                    Input.SpineTurnValue = 0;
                }
            }
        }
    }
}