using System;
using UnityEngine;

namespace MotionCapture.StandTravelModel.Editor
{
    [Serializable]
    public class TuningParameterGroup
    {
        //用于调整节点位置的缩放
        public Vector3 ScaleMotionPos = Vector3.one;
        
        //人物模型根节点位置矫正
        public Vector3 HipPosOffset = Vector3.zero;
        
        //旋转敏度
        public float RotationSensitivity = 1;
        
        //局部位移缩放量
        public Vector3 LocalShiftScale = Vector3.one;
    }
}