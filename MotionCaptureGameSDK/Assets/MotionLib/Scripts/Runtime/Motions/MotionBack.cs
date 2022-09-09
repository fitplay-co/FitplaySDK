using System;
using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using UnityEngine;

namespace MotionLib.Scripts
{
    public class MotionBack : MotionLibBase
    {
        //两侧腕关节Y值接近且接近于肩膀
        //两侧腕关节-肘关节连线接近水平方向&一侧
        //双臂水平向左
        [Header("两侧腕关节Y值和肩膀Y轴最大距离 < WristToShoulderY 时，第一触发条件成立")] [SerializeField] [Range(0, 1)]
        public float HandElowToShoulderY = 0.2f;

        [Header("左手腕，肘，肩和z轴的最大距离 < lWristToCrotchXZ 时，第二触发条件成立")] [SerializeField] [Range(0, 1)]
        public float lWristToShoulderZ = 0.2f;

        [Header("右手腕，肘和z轴的最大距离 < lWristToCrotchXZ 时，第三触发条件成立")] [SerializeField] [Range(0, 1)]
        public float rWristToShoulderZ = 0.56f;


        private void Awake()
        {
            motionMode = MotionLibController.MotionMode.MotionBack;
        }


        /// <summary>
        /// 计算左手手腕手肘到Z轴的距离
        /// </summary>
        /// <param name="leftHand"></param>
        /// <param name="leftElbow"></param>
        /// <param name="leftShoulder"></param>
        /// <returns></returns>
        private bool LeftHandElbowZCorrect(Vector3 leftHand, Vector3 leftElbow, Vector3 leftShoulder)
        {
            if (leftHand.x > leftElbow.x) return false;
            return (leftShoulder.z < lWristToShoulderZ && leftElbow.z < lWristToShoulderZ &&
                    leftHand.z < lWristToShoulderZ);
        }


        /// <summary>
        /// 计算右手手腕手肘到Z轴的距离
        /// </summary>
        /// <param name="rightHand"></param>
        /// <param name="rightElbow"></param>
        /// <param name="rightShoulder"></param>
        /// <returns></returns>
        private bool RightHandElbowZCorrect(Vector3 rightHand, Vector3 rightElbow, Vector3 rightShoulder)
        {
            if (rightHand.x > rightElbow.x || rightHand.x > 0.1f) return false;
            return (rightShoulder.z < rWristToShoulderZ && rightElbow.z < rWristToShoulderZ &&
                    rightHand.z < rWristToShoulderZ);
        }


        /// <summary>
        /// 计算左右手手腕手肘的Y坐标是否和肩膀接近
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="elbow"></param>
        /// <param name="shoulder"></param>
        /// <returns></returns>
        private bool HandElbowYCorrect(Vector3 hand, Vector3 elbow, Vector3 shoulder)
        {
            var handYDis = Math.Abs(shoulder.y - hand.y);
            var elbowYDis = Math.Abs(shoulder.y - elbow.y);
            return (handYDis < HandElowToShoulderY && elbowYDis < HandElowToShoulderY);
        }


        public override void Enabled(bool isEnabled)
        {
            isRunning = isEnabled;
        }

        //肩膀
        private Vector3 leftShoulder;

        private Vector3 rightShoulder;

        //手肘
        private Vector3 leftElbow;

        private Vector3 rightElbow;

        //手腕
        private Vector3 leftHand;
        private Vector3 rightHand;

        public override void CheckMotion(List<Vector3> keyPointList)
        {
            if (!isRunning) return;
            isMotioned = false;
            //肩膀
            leftShoulder = keyPointList[(int) GameKeyPointsType.LeftShoulder];
            rightShoulder = keyPointList[(int) GameKeyPointsType.RightShoulder];
            //手肘
            leftElbow = keyPointList[(int) GameKeyPointsType.LeftElbow];
            rightElbow = keyPointList[(int) GameKeyPointsType.RightElbow];
            //手腕
            leftHand = keyPointList[(int) GameKeyPointsType.LeftHand];
            rightHand = keyPointList[(int) GameKeyPointsType.RightHand];
            //计算 左手腕部肘关节Y值接近肩部
            bool isCorrect = HandElbowYCorrect(leftHand, leftElbow, leftShoulder);
            if (!isCorrect) return;
            //计算 右手腕部肘关节Y值接近肩部
            isCorrect = HandElbowYCorrect(rightHand, rightElbow, rightShoulder);
            if (!isCorrect) return;
            //计算 手腕手肘到Z轴的距离
            isCorrect = LeftHandElbowZCorrect(leftHand, leftElbow, leftShoulder);
            if (!isCorrect) return;
            //计算 右手腕到肩的X,Y轴最小距离
            isCorrect = RightHandElbowZCorrect(rightHand, rightElbow, rightShoulder);
            if (!isCorrect) return;
            isMotioned = true;
            MotionLibEventHandler.DispatchMotionDetectionEvent(motionMode);
            Debug.LogError($"==========YOU ARE IN MOTION TYPE {motionMode} !================");
        }


        private int startYOffset = 50;
        private int startXOffset = 400;


        public void OnGUI()
        {
            if (!isRunning || !isDebug) return;
            GUIStyle labelStyle = new GUIStyle("label");
            labelStyle.fontSize = 40;
            labelStyle.normal.textColor = Color.red;

            GUIStyle labelStyle1 = new GUIStyle("label");
            labelStyle1.fontSize = 20;
            labelStyle1.normal.textColor = Color.blue;
            GUI.Label(new Rect(startXOffset + 20, startYOffset, 600, 80),
                $"左手肩膀坐标:{leftShoulder.x.ToString("0.00")}, {leftShoulder.y.ToString("0.00")}, {leftShoulder.z.ToString("0.00")}",
                labelStyle);
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 40, 600, 80),
                $"左手手肘坐标:{leftElbow.x.ToString("0.00")}, {leftElbow.y.ToString("0.00")}, {leftElbow.z.ToString("0.00")}",
                labelStyle);
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 80, 600, 80),
                $"左手手腕坐标:{leftHand.x.ToString("0.00")}, {leftHand.y.ToString("0.00")}, {leftHand.z.ToString("0.00")}",
                labelStyle);
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 120, 600, 80),
                $"右手肩膀坐标:{rightShoulder.x.ToString("0.00")}, {rightShoulder.y.ToString("0.00")}, {rightShoulder.z.ToString("0.00")}",
                labelStyle);
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 160, 600, 80),
                $"右手手肘坐标:{rightElbow.x.ToString("0.00")}, {rightElbow.y.ToString("0.00")}, {rightElbow.z.ToString("0.00")}",
                labelStyle);
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 200, 600, 80),
                $"右手手腕坐标:{rightHand.x.ToString("0.00")}, {rightHand.y.ToString("0.00")}, {rightHand.z.ToString("0.00")}",
                labelStyle);

            string result = isMotioned ? motionMode.ToString() : "";
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 290, 500, 80),
                $"动作识别为：{result}", labelStyle);


            if (GUI.Button(new Rect(startXOffset + 420, startYOffset - 20, 300, 50), $"当前识别动作：{motionMode}"))
            {
                MotionLibEventHandler.DispatchMotionChangedEvent();
            }


            HandElowToShoulderY = GUI.HorizontalSlider(new Rect(startXOffset + 1000, startYOffset + 45, 300, 40),
                HandElowToShoulderY, 0, 1);
            GUI.Label(new Rect(startXOffset + 620, startYOffset + 40, 400, 80),
                $"左手腕，肘，肩和z轴的最大距离:{HandElowToShoulderY.ToString("0.00")}", labelStyle1);

            lWristToShoulderZ = GUI.HorizontalSlider(new Rect(startXOffset + 1000, startYOffset + 85, 300, 40),
                lWristToShoulderZ, 0, 1);
            GUI.Label(new Rect(startXOffset + 620, startYOffset + 80, 400, 80),
                $"左手腕，肘，肩和z轴的最大距离:{lWristToShoulderZ.ToString("0.00")}", labelStyle1);

            rWristToShoulderZ = GUI.HorizontalSlider(new Rect(startXOffset + 1000, startYOffset + 125, 300, 40),
                rWristToShoulderZ, 0, 1);
            GUI.Label(new Rect(startXOffset + 620, startYOffset + 120, 400, 80),
                $"右手腕，肘和z轴的最大距离:{rWristToShoulderZ.ToString("0.00")}", labelStyle1);
        }
    }
}