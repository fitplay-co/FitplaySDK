using System;
using System.Collections.Generic;
using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using StandTravelModel.Scripts.Runtime;
using UnityEngine;
using UnityEngine.Serialization;
using Input = IMU.Input;

namespace MotionLib.Scripts
{
    public class MotionLibController : MonoBehaviour
    {
        [SerializeField] private StandTravelModelManager standTravelModelManager;
        public List<MotionLibBase> motionList;
        public bool isDebug;

        public MotionMode motionResult = MotionMode.None;
        public MotionMode howToMotion = MotionMode.Motion5;
        private MotionMode lastMotion = MotionMode.None;
        public enum MotionMode 
        {
            None,
            Motion5,
            Motion7,
            Motion7_L,
            MotionBack
        }
        
        /// <summary>
        /// 切换转身模式
        /// </summary>
        public void SwitchMotionMode()
        {
            if (howToMotion == MotionMode.Motion7)
                howToMotion = MotionMode.Motion5;
            else if (howToMotion == MotionMode.Motion5)
                howToMotion = MotionMode.MotionBack;
            else if (howToMotion == MotionMode.MotionBack)
                howToMotion = MotionMode.Motion7;
        }

        private void ChangeMode()
        {
            foreach (MotionLibBase motion in motionList)
            {
                motion.Enabled(motion.motionMode == howToMotion);
            }
        }
        
        private void Start()
        {
            standTravelModelManager = GameObject.FindObjectOfType<StandTravelModelManager>();
            ChangeMode();
            lastMotion = howToMotion;
            foreach (MotionLibBase motion in motionList)
            {
                motion.isDebug = isDebug;
            }
        }

        private void OnValidate()
        {
            foreach (MotionLibBase motion in motionList)
            {
                motion.isDebug = isDebug;
            }
        }

        protected void OnEnable()
        {
            MotionLibEventHandler.onLocalPlayerSpawn += OnLocalSpawn;
            MotionLibEventHandler.onMotionChanged += OnChangedMotion;
        }

        protected void OnDisable()
        {
            MotionLibEventHandler.onLocalPlayerSpawn -= OnLocalSpawn;
            MotionLibEventHandler.onMotionChanged -= OnChangedMotion;
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
        
        
        private void OnChangedMotion()
        {
            SwitchMotionMode();
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

        void FixedUpdate()
        {
            if (standTravelModelManager == null)
            {
                return;
            }
            var keyPointList = standTravelModelManager.GetKeyPointsList();
            //var motionDataModel = standTravelModelManager.motionDataModelReference;
            //var keyRotationList = motionDataModel.GetFitting()?.rotation;

            if (keyPointList != null)
            {
                foreach (MotionLibBase motion in motionList)
                {
                    motion.CheckMotion(keyPointList);
                }
            }

            if (lastMotion != howToMotion)
            {
                Debug.Log("Changed Motion!");
                ChangeMode();
                lastMotion = howToMotion;
            }
        }
    }
}