using System;
using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using MotionLib.Scripts;
using StandTravelModel.Scripts.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace MotionLib.Scripts
{
    public class Motion5 : MotionLibBase
    {
        //两侧手腕X值接近同侧肩部，
        //两侧肘关节夹角约为90°（或45°至90°之间）
        //一侧手腕Y值接近跨部Y值，
        //一侧手腕Y值接近肩部；

        [Header("手腕到肩膀的X轴最大距离 < WristToShoulderX 时，第一触发条件成立")] [SerializeField] [Range(0, 1)]
        public float WristToShoulderX = 0.1f;

        [Header("一侧手腕到跨部的Y轴最大距离 < WristToCrotchY 时，第二触发条件成立")] [SerializeField] [Range(0, 1)]
        public float WristToCrotchY = 0.1f;

        [Header("另一侧手腕到肩膀的Y轴最大距离 < WristToShoulderY 时，第三触发条件成立")] [SerializeField] [Range(0, 1)]
        public float WristToShoulderY = 0.1f;

        [Header("ElbowAngleMin < 两侧肘关节夹角 < ElbowAngleMax 时，第四触发条件成立")] [SerializeField] [Range(0, 180)]
        public float ElbowAngleMin = 45;

        [Header("ElbowAngleMin < 两侧肘关节夹角 < ElbowAngleMax 时，第四触发条件成立")] [SerializeField] [Range(0, 180)]
        public float ElbowAngleMax = 110;

        [Header("手肘距离检测模式：手肘到肩膀的最大距离 < WristToShoulder")] [SerializeField] [Range(0, 1)]
        public float WristToShoulder = 0.4f;

        [Header("手肘距离检测模式：手肘到胯部的最大距离 < WristToShoulder")] [SerializeField] [Range(0, 1)]
        public float WristToCrotch = 0.4f;
        

        private Motion5Type motion5Type = Motion5Type.None;

        private CheckMode motionnCheckMode = CheckMode.Andgle;

        public enum Motion5Type
        {
            None,
            Left,
            Right
        }

        public enum CheckMode
        {
            Andgle,
            Distance
        }

        private void Awake()
        {
            motionMode = MotionLibController.MotionMode.Motion5;
        }

 
        /// <summary>
        /// 适用于角色直接放在场景中
        /// </summary>
        private Motion5Type motionType;

        private float leftXDis;
        private float rightXDis;

        private bool WristToShoulderXCorrect(Vector3 leftElbow, Vector3 rightElbow, Vector3 leftShoulder,
            Vector3 rightShoulder)
        {
            leftXDis = Math.Abs(leftShoulder.x - leftElbow.x);
            if (leftXDis > WristToShoulderX)
                return false;
            rightXDis = Math.Abs(rightShoulder.x - rightElbow.x);
            if (rightXDis > WristToShoulderX)
                return false;
            return true;
        }

        private float angleL;
        private float angleR;

        private bool ElbowsAngleCorrect(Vector3 leftHand, Vector3 rightHand, Vector3 leftElbow, Vector3 rightElbow,
            Vector3 leftShoulder, Vector3 rightShoulder)
        {
            angleL = Vector3.Angle(leftShoulder - leftElbow, leftHand - leftElbow);
            if (angleL > ElbowAngleMin && angleL < ElbowAngleMax)
            {
                angleR = Vector3.Angle(rightShoulder - rightElbow, rightHand - rightElbow);
                if (angleR > ElbowAngleMin && angleR < ElbowAngleMax)
                {
                    return true;
                }
            }

            return false;
        }

        private float leftHandToHipDis;
        private float leftHandSToShoulderDis;

        private float rightHandToHipDis;
        private float rightHandSToShoulderDis;

        private bool WristDistanceCorrect(Vector3 leftHand, Vector3 rightHand, Vector3 leftHip, Vector3 rightHip,
            Vector3 leftShoulder, Vector3 rightShoulder)
        {
            leftHandSToShoulderDis = Math.Abs(Vector3.Distance(leftShoulder, leftHand));
            rightHandToHipDis = Math.Abs(Vector3.Distance(rightHip, rightHand));
            if (leftHandSToShoulderDis < WristToShoulder && rightHandToHipDis < WristToCrotch)
                return true;
            leftHandToHipDis = Math.Abs(Vector3.Distance(leftHip, leftHand));
            rightHandSToShoulderDis = Math.Abs(Vector3.Distance(rightShoulder, rightHand));
            if (rightHandSToShoulderDis < WristToShoulder && leftHandToHipDis < WristToCrotch)
                return true;
            return false;
        }

        private float leftShoulderYDis;
        private float rightShoulderYDis;
        private float leftHipYDis;
        private float rightHipYDis;

        private Motion5Type WristToShoulderYCorrect(Vector3 leftHand, Vector3 rightHand, Vector3 leftHip,
            Vector3 rightHip,
            Vector3 leftShoulder, Vector3 rightShoulder)
        {
            leftShoulderYDis = Math.Abs(leftShoulder.y - leftHand.y);
            leftHipYDis = Math.Abs(leftHip.y - leftHand.y);
            rightShoulderYDis = Math.Abs(rightShoulder.y - rightHand.y);
            rightHipYDis = Math.Abs(rightHip.y - rightHand.y);
            if (leftShoulderYDis < WristToShoulderY && rightHipYDis < WristToCrotchY)
                return Motion5Type.Left;

            if (rightShoulderYDis < WristToShoulderY && leftHipYDis < WristToCrotchY)
                return Motion5Type.Right;

            return Motion5Type.None;
        }

        private void SwitchCheckMode()
        {
            if (motionnCheckMode == CheckMode.Andgle)
                motionnCheckMode = CheckMode.Distance;
            else
                motionnCheckMode = CheckMode.Andgle;
        }

        public override void Enabled(bool isEnabled)
        {
            isRunning = isEnabled;
        }

        public override void CheckMotion(List<Vector3> keyPointList)
        {
            if (!isRunning) return;
            //肩膀
            var leftShoulder = keyPointList[(int) GameKeyPointsType.LeftShoulder];
            var rightShoulder = keyPointList[(int) GameKeyPointsType.RightShoulder];
            //胯部
            var leftHip = keyPointList[(int) GameKeyPointsType.LeftHip];
            var rightHip = keyPointList[(int) GameKeyPointsType.RightHip];
            //手肘
            var leftElbow = keyPointList[(int) GameKeyPointsType.LeftElbow];
            var rightElbow = keyPointList[(int) GameKeyPointsType.RightElbow];
            //手腕
            var leftHand = keyPointList[(int) GameKeyPointsType.LeftHand];
            var rightHand = keyPointList[(int) GameKeyPointsType.RightHand];

            //计算 手腕X轴和肩膀X轴的距离是否正确
            bool isCorrect = WristToShoulderXCorrect(leftElbow, rightElbow, leftShoulder, rightShoulder);
            if (!isCorrect)
            {
                isMotioned = false;
                return;
            }
            //Debug.Log("left and Right Wrist X is Correct!");
            //计算 两侧手肘的夹角
            if (motionnCheckMode == CheckMode.Andgle)
            {
                isCorrect = ElbowsAngleCorrect(leftHand, rightHand, leftElbow, rightElbow, leftShoulder,
                    rightShoulder);
            }
            else
            {
                isCorrect = WristDistanceCorrect(leftHand, rightHand, leftHip, rightHip, leftShoulder,
                    rightShoulder);
            }

            if (!isCorrect)
            {
                
                isMotioned = false;
                return;
            }
            //Debug.Log("left and Right Elbow is Correct!");
            motionType =
                WristToShoulderYCorrect(leftHand, rightHand, leftHip, rightHip, leftShoulder, rightShoulder);
            if (motionType != Motion5Type.None)
            {
                isMotioned = true;//MotionMode.Motion5;
                //Debug.LogError("==========YOU ARE IN MOTION TYPE 5 MODE!================");
            }
            else
            {
                isMotioned = false;
                //motionResult = MotionMode.None;
            }
        }


        private int startYOffset = 50;
        private int startXOffset = 800;


        public void OnGUI()
        {
            if (!isRunning) return;
            GUIStyle labelStyle = new GUIStyle("label");
            labelStyle.fontSize = 40;
            labelStyle.normal.textColor = Color.red;

            GUIStyle labelStyle1 = new GUIStyle("label");
            labelStyle1.fontSize = 20;
            labelStyle1.normal.textColor = Color.blue;
            string checkMode = (motionnCheckMode == Motion5.CheckMode.Andgle) ? "当前为肘夹角检测模式" : "当前为手腕距离检测模式";
            GUI.Label(new Rect(startXOffset + 20, startYOffset, 400, 80),
                $"左侧手X轴距离:{leftXDis.ToString("0.00")}", labelStyle);
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 40, 400, 80),
                $"右侧手X轴距离:{rightXDis.ToString("0.00")}", labelStyle);

            GUI.Label(new Rect(startXOffset + 20, startYOffset + 80, 400, 80),
                $"左侧手腕到肩:{leftShoulderYDis.ToString("0.00")}", labelStyle);
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 120, 400, 80),
                $"左侧手腕到胯:{leftHipYDis.ToString("0.00")}", labelStyle);
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 160, 400, 80),
                $"右侧手腕到肩:{rightShoulderYDis.ToString("0.00")}", labelStyle);
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 200, 400, 80),
                $"右侧手腕到胯:{rightHipYDis.ToString("0.00")}", labelStyle);


            if (motionnCheckMode == Motion5.CheckMode.Andgle)
            {
                GUI.Label(new Rect(startXOffset + 20, startYOffset + 240, 400, 80),
                    $"左侧手肘夹角:{angleL.ToString("0.00")}", labelStyle);
                GUI.Label(new Rect(startXOffset + 20, startYOffset + 280, 400, 80),
                    $"右侧手肘夹角:{angleR.ToString("0.00")}", labelStyle);
            }
            else
            {
                GUI.Label(new Rect(startXOffset + 20, startYOffset + 240, 400, 80),
                    $"左手到胯的距离:{leftHandToHipDis.ToString("0.00")}", labelStyle);
                GUI.Label(new Rect(startXOffset + 20, startYOffset + 280, 400, 80),
                    $"左手到肩的距离:{leftHandSToShoulderDis.ToString("0.00")}", labelStyle);

                GUI.Label(new Rect(startXOffset + 20, startYOffset + 320, 400, 80),
                    $"右手到胯的距离:{rightHandToHipDis.ToString("0.00")}", labelStyle);
                GUI.Label(new Rect(startXOffset + 20, startYOffset + 360, 400, 80),
                    $"右手到肩的距离:{rightHandSToShoulderDis.ToString("0.00")}", labelStyle);
            }

            if (motionType != Motion5.Motion5Type.None)
            {
                string result = (motionType == Motion5.Motion5Type.Left) ? "左手高于右手" : "右手高于左手";
                GUI.Label(new Rect(startXOffset + 20, startYOffset + 400, 500, 80),
                    $"双手姿态:{result}", labelStyle);
            }

            string done = isMotioned ? motionMode.ToString() : "";
            GUI.Label(new Rect(startXOffset + 20, startYOffset + 450, 400, 80),
                $"动作识别为：{done}", labelStyle);

            if (GUI.Button(new Rect(startXOffset + 720, startYOffset - 20, 300, 50), checkMode))
            {
                SwitchCheckMode();
            }
            
            if (GUI.Button(new Rect(startXOffset + 420, startYOffset - 20, 300, 50), $"当前识别动作：{motionMode}"))
            {
                MotionLibEventHandler.DispatchMotionChangedEvent();
            }

            WristToShoulderX = GUI.HorizontalSlider(new Rect(startXOffset + 720, startYOffset + 45, 300, 40),
                WristToShoulderX, 0, 1);
            GUI.Label(new Rect(startXOffset + 420, startYOffset + 40, 300, 80),
                $"手腕到肩膀X轴的最大距离:{WristToShoulderX.ToString("0.00")}", labelStyle1);

            WristToCrotchY = GUI.HorizontalSlider(new Rect(startXOffset + 720, startYOffset + 85, 300, 40),
                WristToCrotchY, 0, 1);
            GUI.Label(new Rect(startXOffset + 420, startYOffset + 80, 300, 80),
                $"手腕到跨部Y轴的最大距离:{WristToCrotchY.ToString("0.00")}", labelStyle1);

            WristToShoulderY = GUI.HorizontalSlider(new Rect(startXOffset + 720, startYOffset + 125, 300, 40),
                WristToShoulderY, 0, 1);
            GUI.Label(new Rect(startXOffset + 420, startYOffset + 120, 300, 80),
                $"手腕到肩膀Y轴最大距离:{WristToShoulderY.ToString("0.00")}", labelStyle1);

            if (motionnCheckMode == Motion5.CheckMode.Andgle)
            {
                ElbowAngleMin = GUI.HorizontalSlider(new Rect(startXOffset + 720, startYOffset + 165, 300, 40),
                    ElbowAngleMin, 0, 180);
                GUI.Label(new Rect(startXOffset + 420, startYOffset + 160, 300, 80),
                    $"两侧肘关节最小夹角:{ElbowAngleMin.ToString("0.00")}", labelStyle1);

                ElbowAngleMax = GUI.HorizontalSlider(new Rect(startXOffset + 720, startYOffset + 205, 300, 40),
                    ElbowAngleMax, 0, 180);
                GUI.Label(new Rect(startXOffset + 420, startYOffset + 200, 300, 80),
                    $"两侧肘关节最小夹角:{ElbowAngleMax.ToString("0.00")}", labelStyle1);
            }
            else
            {
                WristToShoulder = GUI.HorizontalSlider(new Rect(startXOffset + 720, startYOffset + 165, 300, 40),
                    WristToShoulder, 0, 1);
                GUI.Label(new Rect(startXOffset + 420, startYOffset + 160, 300, 80),
                    $"手肘到肩膀的最大距离:{WristToShoulder.ToString("0.00")}", labelStyle1);

                WristToCrotch = GUI.HorizontalSlider(new Rect(startXOffset + 720, startYOffset + 205, 300, 40),
                    WristToCrotch, 0, 1);
                GUI.Label(new Rect(startXOffset + 420, startYOffset + 200, 300, 80),
                    $"手肘到胯部的最大距离:{WristToCrotch.ToString("0.00")}", labelStyle1);
            }
        }
    }
}