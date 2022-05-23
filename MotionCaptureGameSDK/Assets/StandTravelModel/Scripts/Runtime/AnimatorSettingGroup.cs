using System;
using System.Collections.Generic;
using UnityEngine;

namespace MotionCapture.StandTravelModel.Editor
{
    [Serializable]
    public class AnimatorSettingGroup
    {
        public List<RunAnimatorParams> runAnimationSettings;

        [Serializable]
        public class RunAnimatorParams
        {
            public AnimationList animation;
            [Range(0.1f, 4f)] public float playBackSpeed;
            public float movingSpeed;
            public float cadenceThreshold;
        }
        
        [Serializable]
        public enum AnimationList
        {
            Run = 0,
            Sprint
        }
    }

    public class AnimationStatement
    {
        public string AnimationState;
        public string TransitionParameter;
        public string AnimationPlaySpeedMultiplier;
    }
}