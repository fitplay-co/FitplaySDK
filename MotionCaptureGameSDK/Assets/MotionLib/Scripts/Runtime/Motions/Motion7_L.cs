using System;
using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using UnityEngine;

namespace MotionLib.Scripts
{
    public class Motion7_L : MotionLibBase
    {
        //左手腕部X值Y值均大于肩部
        //小臂有一个向前挥动的速度
        //右手腕部X值Z值接近肩部
        //上臂-小臂夹角接近180°

        [Header("左手腕到肩的X,Y轴最小距离 < WristToShoulderXY 时，第一触发条件成立")] [SerializeField] [Range(0, 1)]
        public float lWristToShoulderXY = 0.1f;

        [Header("右手腕到肩的X,Z轴最小距离 < lWristToCrotchXZ 时，第二触发条件成立")] [SerializeField] [Range(0, 1)]
        public float rWristToCrotchXZ = 0.1f;

        [Header("ElbowAngleMin < 右侧肘关节夹角 时，第三触发条件成立")] [SerializeField] [Range(0, 180)]
        public float ElbowAngleMin = 160;

        [Header("左手向前的最小速度 < MoveMinSpeed 时，第四触发条件成立")] [SerializeField] [Range(0, 50)]
        public float MoveMinSpeed = 10;


        

        private void Awake()
        {
            motionMode = MotionLibController.MotionMode.Motion7_L;
        }

        private float leftHandXDis;
        private float leftHandYDis;
        private bool HandToShoulderXZCorrect(Vector3 leftHand, Vector3 leftShoulder)
        {
            leftHandXDis = leftHand.x - leftShoulder.x;
            leftHandYDis = leftHand.y - leftShoulder.y;
            return (leftHandXDis > lWristToShoulderXY && leftHandYDis > lWristToShoulderXY);
        }

        private float rightHandXDis;
        private float rightHandZDis;

        private bool RightHandXZCorrect(Vector3 rightHand, Vector3 rightShoulder)
        {
            rightHandXDis = Math.Abs(rightShoulder.x - rightHand.x);
            rightHandZDis = Math.Abs(rightShoulder.z - rightHand.z);
            return (leftHandXDis < rWristToCrotchXZ && rightHandZDis < rWristToCrotchXZ);
        }

        private float angleL;
        private bool RightElbowAngleCorrect(Vector3 rightHand, Vector3 rightElbow, Vector3 rightShoulder)
        {
            angleL = Vector3.Angle(rightShoulder - rightElbow, rightHand - rightElbow);
            return (angleL > ElbowAngleMin);
        }

        private float speed;

        private struct HandMoveData
        {
            public float deltaTime;
            public float zValue;
        }

        private List<HandMoveData> leftHandMoveList = new List<HandMoveData>();

        private bool LeftHandMovingCorrect(Vector3 leftHand)
        {
            leftHandMoveList.Add(new HandMoveData() {deltaTime = Time.deltaTime, zValue = leftHand.z});
            if (leftHandMoveList.Count < 2) return false;
            float allTimes = 0;
            //保持最多30帧的数据
            if(leftHandMoveList.Count > 30)
                leftHandMoveList.RemoveAt(0);
            for (int i = 1; i < leftHandMoveList.Count; i++)
            {
                allTimes += leftHandMoveList[i].deltaTime;
            }

            float allDistances = leftHandMoveList[leftHandMoveList.Count - 1].zValue - leftHandMoveList[0].zValue;

            speed = (allDistances * 100) / allTimes;
            //Debug.LogError($"allDistances:{allDistances}, allTimes:{allTimes}, rightHand.y:{rightHand.y},rightHand.z:{rightHand.z}");
            return (speed > MoveMinSpeed);
        }

        public override void Enabled(bool isEnabled)
        {
            isRunning = isEnabled;
        }

        private Vector3 leftHand;
        public override void CheckMotion(List<Vector3> keyPointList)
        {
            if (!isRunning) return;
            //肩膀
            var leftShoulder = keyPointList[(int) GameKeyPointsType.LeftShoulder];
            var rightShoulder = keyPointList[(int) GameKeyPointsType.RightShoulder];
            //手肘
            var leftElbow = keyPointList[(int) GameKeyPointsType.LeftElbow];
            var rightElbow = keyPointList[(int) GameKeyPointsType.RightElbow];
            //手腕
            leftHand = keyPointList[(int) GameKeyPointsType.LeftHand];
            var rightHand = keyPointList[(int) GameKeyPointsType.RightHand];
            //rightHand.z += rightHand.y * -0.1f;
            //var leftHandQua = keyRotationList[8].Rotation();
            //var leftElbowQua = keyRotationList[6].Rotation();
            //var leftShoulderQua = keyRotationList[2].Rotation();
            //计算 右手腕部X值Z值接近肩部
            bool isCorrect = RightHandXZCorrect(rightHand, rightShoulder);
            if (!isCorrect)
            {
                leftHandMoveList = new List<HandMoveData>();
                isMotioned = false;
                return;
            }

            //计算 右手上臂-小臂夹角接近180°
            //LeftElbowAngleCorrect2(leftShoulderQua, leftElbowQua);
            isCorrect = RightElbowAngleCorrect(rightHand, rightElbow, rightShoulder);
            if (!isCorrect)
            {
                leftHandMoveList = new List<HandMoveData>();
                isMotioned = false;
                return;
            }

            //计算 左手腕到肩的X,Y轴最小距离
            isCorrect = HandToShoulderXZCorrect(leftHand, leftShoulder);
            if (!isCorrect)
            {
                leftHandMoveList = new List<HandMoveData>();
                isMotioned = false;
                return;
            }


            //计算 小臂向前的移动速度
            isCorrect = LeftHandMovingCorrect(leftHand);
            if (isCorrect)
            {
                isMotioned = true;
                MotionLibEventHandler.DispatchMotionDetectionEvent(motionMode);
                // Debug.LogError("==========YOU ARE IN MOTION TYPE 7 MODE!================");
            }
            else
                isMotioned = false;
        }


        private int startYOffset = 80;
        private int startXOffset = 800;


        public void OnGUI()
        {
            if (!isRunning || !isDebug) return;
            GUIStyle labelStyle = new GUIStyle("label");
            labelStyle.fontSize = 40;
            labelStyle.normal.textColor = Color.red;

            GUIStyle labelStyle1 = new GUIStyle("label");
            labelStyle1.fontSize = 20;
            labelStyle1.normal.textColor = Color.blue;
            GUI.Label(new Rect(startXOffset + 20, startYOffset, 400, 80),
                $"左侧手X轴距离:{leftHandXDis.ToString("0.00")}", labelStyle);
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 40, 400, 80),
                $"左侧手Y轴距离:{leftHandYDis.ToString("0.00")}", labelStyle);

            GUI.Label(new Rect(startXOffset + 20, startYOffset + 80, 400, 80),
                $"右侧手X轴距离:{rightHandXDis.ToString("0.00")}", labelStyle);
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 120, 400, 80),
                $"右侧手Z轴距离:{rightHandZDis.ToString("0.00")}", labelStyle);
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 160, 500, 80),
                $"右侧手肘夹角:{angleL.ToString("0.00")}", labelStyle);
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 200, 400, 80),
                $"左侧手移动速度:{speed.ToString("0.00")}", labelStyle);
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 240, 600, 80),
                $"左侧手坐标:{leftHand.x.ToString("0.00")}, {leftHand.y.ToString("0.00")}, {leftHand.z.ToString("0.00")}", labelStyle);
            string result = isMotioned ? motionMode.ToString() : "";
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 290, 400, 80),
                $"动作识别为：{result}", labelStyle);


            if (GUI.Button(new Rect(startXOffset + 420, startYOffset - 20, 300, 50), $"当前识别动作：{motionMode}"))
            {
                MotionLibEventHandler.DispatchMotionChangedEvent();
            }

            lWristToShoulderXY = GUI.HorizontalSlider(new Rect(startXOffset + 720, startYOffset + 45, 300, 40),
                lWristToShoulderXY, 0, 1);
            GUI.Label(new Rect(startXOffset + 420, startYOffset + 40, 300, 80),
                $"左手腕到肩X,Y轴的最大距离:{lWristToShoulderXY.ToString("0.00")}", labelStyle1);

            rWristToCrotchXZ = GUI.HorizontalSlider(new Rect(startXOffset + 720, startYOffset + 85, 300, 40),
                rWristToCrotchXZ, 0, 1);
            GUI.Label(new Rect(startXOffset + 420, startYOffset + 80, 300, 80),
                $"右手腕到肩X,Z轴的最大距离:{rWristToCrotchXZ.ToString("0.00")}", labelStyle1);

            ElbowAngleMin = GUI.HorizontalSlider(new Rect(startXOffset + 720, startYOffset + 125, 300, 40),
                ElbowAngleMin, 0, 180);
            GUI.Label(new Rect(startXOffset + 420, startYOffset + 120, 300, 80),
                $"右侧肘关节夹角:{ElbowAngleMin.ToString("0.00")}", labelStyle1);

            MoveMinSpeed = GUI.HorizontalSlider(new Rect(startXOffset + 720, startYOffset + 165, 300, 40),
                MoveMinSpeed, 0, 50);
            GUI.Label(new Rect(startXOffset + 420, startYOffset + 160, 300, 80),
                $"左手移动速度:{MoveMinSpeed.ToString("0.00")}", labelStyle1);
        }
    }
}