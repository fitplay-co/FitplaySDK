using System;
using System.Collections.Generic;
using UnityEngine;

namespace StandTravelModel
{
    [Serializable]
    public class AnimatorSettingGroup
    {
        public List<AnimationStatement> animationMap
            = new List<AnimationStatement>
            {
                new AnimationStatement
                {
                    animType = AnimationList.Idle, animParam = new AnimatorParamStct
                    {
                        stateName = "Idle",
                        stateTransition = "",
                        playSpeedMultiplier = "",
                        playBackSpeed = 1
                    }
                },
                new AnimationStatement
                {
                    animType = AnimationList.LeftStep, animParam = new AnimatorParamStct
                    {
                        stateName = "LeftStep",
                        stateTransition = "isLeftStep",
                        playSpeedMultiplier = "",
                        playBackSpeed = 1
                    }
                },
                new AnimationStatement
                {
                    animType = AnimationList.RightStep, animParam = new AnimatorParamStct
                    {
                        stateName = "RightStep",
                        stateTransition = "isRightStep",
                        playSpeedMultiplier = "",
                        playBackSpeed = 1
                    }
                },
                new AnimationStatement
                {
                    animType = AnimationList.Run, animParam = new AnimatorParamStct
                    {
                        stateName = "Run",
                        stateTransition = "isRun",
                        playSpeedMultiplier = "",
                        playBackSpeed = 1
                    }
                },
                new AnimationStatement
                {
                    animType = AnimationList.Jump, animParam = new AnimatorParamStct
                    {
                        stateName = "Jump",
                        stateTransition = "isJump",
                        playSpeedMultiplier = "",
                        playBackSpeed = 1
                    }
                },
                new AnimationStatement
                {
                    animType = AnimationList.Squat, animParam = new AnimatorParamStct
                    {
                        stateName = "Squat",
                        stateTransition = "isSquat",
                        playSpeedMultiplier = "",
                        playBackSpeed = 1
                    }
                },
            };

        /*public List<AnimationTransitionStatement> animationTransitionMap
            = new List<AnimationTransitionStatement>()
            {
                new AnimationTransitionStatement()
                    {animState1 = AnimationList.Idle, animState2 = AnimationList.LeftStep, transParameter = "idle2LeftStep"},
                new AnimationTransitionStatement()
                    {animState1 = AnimationList.Idle, animState2 = AnimationList.RightStep, transParameter = "idle2RightStep"},
                new AnimationTransitionStatement()
                    {animState1 = AnimationList.Idle, animState2 = AnimationList.Jump, transParameter = "idle2Jump"},
                new AnimationTransitionStatement()
                    {animState1 = AnimationList.Idle, animState2 = AnimationList.Squat, transParameter = "idle2Squat"},
                new AnimationTransitionStatement()
                    {animState1 = AnimationList.LeftStep, animState2 = AnimationList.Run, transParameter = "LeftStep2Run"},
                new AnimationTransitionStatement()
                    {animState1 = AnimationList.RightStep, animState2 = AnimationList.Run, transParameter = "RightStep2Run"},
                new AnimationTransitionStatement()
                    {animState1 = AnimationList.Run, animState2 = AnimationList.Jump, transParameter = "Run2Jump"}
            };*/
    }
    
    [Serializable]
    public enum AnimationList
    {
        Idle = 0,
        LeftStep,
        RightStep,
        Run,
        Jump,
        Squat,
        Custom,
        
        Count = Custom+1
    }

    [Serializable]
    public class AnimationStatement
    {
        public AnimationList animType;
        public AnimatorParamStct animParam;
    }

    [Serializable]
    public struct AnimatorParamStct
    {
        public string stateName;
        public string stateTransition;
        public string playSpeedMultiplier;
        [Range(0.1f, 4f)] public float playBackSpeed;
    }

    /*[Serializable]
    public class AnimationTransitionStatement
    {
        public AnimationList animState1;
        public AnimationList animState2;
        public string transParameter;
    }*/
}