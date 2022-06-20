using System;
using RootMotion.FinalIK;
using StandTravelModel.Core;
using UnityEngine;

namespace StandTravelModel
{
    [Serializable]
    public class ModelIKSettingGroup
    {
        //用于描绘接收到33个点的信息
        public GameObject NodePrefab;
        
        //人物IK控制脚本组件，需要指定挂在人物模型上的该组件
        public UnityIKMecanimScript IKScript;
        
        //final ik component
        public FullBodyBipedIK FinalIKComponent;
        public LookAtIK FinalIKLookAtComponent;

        public void SetEnable(bool active)
        {
            FinalIKComponent.enabled = active;
            FinalIKLookAtComponent.enabled = active;
        }
    }
}