using System;
using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using StandTravelModel.Scripts.Runtime;
using UnityEngine;
using UnityEngine.Serialization;
using Input = IMU.Input;

namespace MotionLib.Scripts
{
    public class MotionLibController : MonoBehaviour
    {
        //两侧手腕X值接近同侧肩部，
        //两侧肘关节夹角约为90°（或45°至90°之间）
        //一侧手腕Y值接近跨部Y值，
        //一侧手腕Y值接近肩部；
        [SerializeField] private StandTravelModelManager standTravelModelManager;

        [Header("手腕到肩膀的X轴距离 < WristToShoulderX 时，第一触发条件成立")] [SerializeField] [Range(0, 1)]
        public float WristToShoulderX = 0.1f;

        [Header("一侧手腕到跨部的Y轴距离 < WristToCrotchY 时，第二触发条件成立")] [SerializeField] [Range(0, 1)]
        public float WristToCrotchY = 0.1f;

        [Header("另一侧手腕到肩膀的Y轴距离 < WristToShoulderY 时，第三触发条件成立")] [SerializeField] [Range(0, 1)]
        public float WristToShoulderY = 0.1f;

        [Header("ElbowAngleMin < 两侧肘关节夹角 < ElbowAngleMax 时，第四触发条件成立")] [SerializeField] [Range(0, 90)]
        public float ElbowAngleMin = 45;

        [Header("ElbowAngleMin < 两侧肘关节夹角 < ElbowAngleMax 时，第四触发条件成立")] [SerializeField] [Range(0, 90)]
        public float ElbowAngleMax = 90;

        [FormerlySerializedAs("HowToTurn")] [SerializeField]
        public MotionMode howToMotion = MotionMode.Motion5;

        private Motion5Type motion5Type = Motion5Type.None;

        //private Vector3 leftShoulder;
        //private Vector3 rightShoulder;
        //private Vector3 leftHip;
        //private Vector3 rightHip;
        //private Vector3 centerHip;
        //private Vector3 centerShoulder;

        public enum Motion5Type
        {
            None,
            Left,
            Right
        }

        public enum MotionMode
        {
            Motion5,
            Motion7
        }

        /// <summary>
        /// 适用于角色直接放在场景中
        /// </summary>
        private void Start()
        {
            standTravelModelManager = GameObject.FindObjectOfType<StandTravelModelManager>();
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = Color.red;
        }

        protected void OnEnable()
        {
            MotionLibEventHandler.onLocalPlayerSpawn += OnLocalSpawn;
        }

        protected void OnDisable()
        {
            MotionLibEventHandler.onLocalPlayerSpawn -= OnLocalSpawn;
        }

        /// <summary>
        /// 适用于角色是动态创建出来，并且角色名为"LocalPlayer",StandTravelModelManager在子节点上
        /// </summary>
        private void OnLocalSpawn()
        {
            if (standTravelModelManager != null) return;
            standTravelModelManager = GameObject.Find("LocalPlayer").transform
                .GetComponentInChildren<StandTravelModelManager>();
        }

        /// <summary>
        /// 切换转身模式
        /// </summary>
        public void SwitchTurnMode()
        {
            if (howToMotion == MotionMode.Motion7)
                howToMotion = MotionMode.Motion5;
            else
                howToMotion = MotionMode.Motion7;
        }


        /// <summary>
        /// 模式检测
        /// </summary>
        /// <returns></returns>
        bool IsTraveMode()
        {
            return (standTravelModelManager != null &&
                    standTravelModelManager.currentMode ==
                    StandTravelModel.Scripts.Runtime.MotionMode.Travel); //standTravelModelManager.Enabled &&
        }

        private Motion5Type motionType1;
        private Motion5Type motionType2;
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
            //Debug.Log($"============AngleL:{angleL}============");
            if (angleL > ElbowAngleMin && angleL < ElbowAngleMax)
            {
                angleR = Vector3.Angle(rightShoulder - rightElbow, rightHand - rightElbow);
                //Debug.Log($"============AngleR:{angleR}============");
                if (angleR > ElbowAngleMin && angleR < ElbowAngleMax)
                {
                    return true;
                }
            }

            return false;
        }

        private Motion5Type WristToShoulderYCorrect(Vector3 hand, Vector3 shoulder, Vector3 hip, bool isLeft)
        {
            if (Math.Abs(shoulder.y - hand.y) < WristToShoulderY)
            {
                return (isLeft) ? Motion5Type.Left : Motion5Type.Right;
            }
            else if (Math.Abs(hip.y - hand.y) < WristToShoulderY)
            {
                return (isLeft) ? Motion5Type.Right : Motion5Type.Left;
            }

            return Motion5Type.None;
        }


        void FixedUpdate()
        {
            if (IsTraveMode())
            {
                var keyPointList = standTravelModelManager.GetKeyPointsList();
                if (keyPointList == null) return;
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
                if (!isCorrect) return;
                //Debug.Log("left and Right Wrist X is Correct!");
                //计算 两侧手肘的夹角
                isCorrect = ElbowsAngleCorrect(leftHand, rightHand, leftElbow, rightElbow, leftShoulder, rightShoulder);
                if (!isCorrect) return;
                //Debug.Log("left and Right Elbow is Correct!");
                motionType1 = WristToShoulderYCorrect(leftHand, leftShoulder, leftHip, true);
                motionType2 = WristToShoulderYCorrect(rightHand, rightShoulder, rightHip, false);
                if (motionType1 == Motion5Type.None || motionType2 == Motion5Type.None)
                {
                    Debug.LogError("motionType1 OR motionType2 ERROR!");
                    return;
                }

                if (motionType1 != motionType2)
                {
                    Debug.LogError("==========YOU ARE IN MOTION TYPE 5 MODE!================");
                }
            }
        }


        private int startYOffset = 650;
        private GUIStyle labelStyle = new GUIStyle("label");


        public void OnGUI()
        {
            if (standTravelModelManager == null)
            {
                return;
            }

            GUI.Label(new Rect(20, startYOffset, 400, 80),
                $"左侧手X轴距离:{leftXDis.ToString("0.00")}", labelStyle);
            GUI.Label(new Rect(20, startYOffset + 40, 400, 80),
                $"右侧手X轴距离:{angleR.ToString("0.00")}", labelStyle);

            GUI.Label(new Rect(20, startYOffset + 80, 400, 80),
                $"左侧手肘夹角:{angleL.ToString("0.00")}", labelStyle);
            GUI.Label(new Rect(20, startYOffset + 120, 400, 80),
                $"右侧手肘夹角:{angleR.ToString("0.00")}", labelStyle);
            GUI.Label(new Rect(20, startYOffset + 160, 400, 80),
                $"左侧手位置:{motionType1}", labelStyle);
            GUI.Label(new Rect(20, startYOffset + 200, 400, 80),
                $"右侧手位置:{motionType2}", labelStyle);
        }
    }
}