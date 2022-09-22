using System;
using RootMotion.FinalIK;
using StandTravelModel.Scripts.Runtime.Core;

namespace StandTravelModel.Scripts.Runtime
{
    [Serializable]
    public class ModelIKSettingGroup
    {
        //人物IK控制脚本组件，需要指定挂在人物模型上的该组件
        public UnityIKMecanimScript IKScript;
        
        //final ik component
        public FullBodyBipedIK FinalIKComponent;
        public LookAtIK FinalIKLookAtComponent;

        public void SetEnable(bool active)
        {
            FinalIKComponent.enabled = active;
            IKScript.enabled = active;
            //FinalIKLookAtComponent.enabled = active;
            if(!active)
            {
                FinalIKLookAtComponent.enabled = active;
            }
        }
    }
}