using UnityEngine;

namespace StandTravelModel.Core
{
    public class CharacterAnimatorController
    {
        private Animator animator;
        
        public CharacterAnimatorController(Animator animator)
        {
            this.animator = animator;
        }

        public void PlayAnimation(string stateName, string transitionParameter)
        {
            int stateHash = Animator.StringToHash(stateName);
            int parameterHash = Animator.StringToHash(transitionParameter);
            if (animator.HasState(0, stateHash))
            {
                if (!animator.GetBool(parameterHash))
                {
                    animator.Play(stateHash, 0, 0);
                    animator.SetBool(parameterHash, true);
                }
            }
            else
            {
                Debug.LogError($"The animator start \"{stateName}\" was not found");
            }
        }

        public void StopAnimation(string transitionParameter)
        {
            if (transitionParameter == "")
            {
                return;
            }

            int parameterHash = Animator.StringToHash(transitionParameter);
            if (animator.GetBool(parameterHash))
            {
                animator.SetBool(parameterHash, false);
            }
        }

        public void SetAnimationPlaySpeed(string parameterName, float speed)
        {
            int parameterHash = Animator.StringToHash(parameterName);
            animator.SetFloat(parameterName, speed);
        }
        
    }
}