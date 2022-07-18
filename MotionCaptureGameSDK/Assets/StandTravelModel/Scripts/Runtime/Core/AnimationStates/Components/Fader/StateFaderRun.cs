using System;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class StateFaderRun : StateFader
    {
        private int animIdRunTransition;
        private Animator animator;

        public StateFaderRun(Animator animator, float fadeSpeed, string key) : base(fadeSpeed)
        {
            this.animator = animator;
            this.animIdRunTransition = Animator.StringToHash(key);
        }

        protected override void OnProgress(float percent)
        {
            animator.SetFloat(animIdRunTransition, percent);
        }
    }
}