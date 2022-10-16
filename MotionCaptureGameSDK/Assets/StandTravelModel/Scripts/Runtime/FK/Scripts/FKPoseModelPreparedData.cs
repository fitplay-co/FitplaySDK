using System;
using System.Collections;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class FKPoseModelPreparedData : MonoBehaviour
    {
        [SerializeField] private Quaternion[] rotationCorrects;

        public Quaternion[] GetRotationCorrects()
        {
            return rotationCorrects;
        }

        public void SetRotationCorrects(Quaternion[] rotationCorrects)
        {
            this.rotationCorrects = rotationCorrects;
        }

        public void BakeData()
        {
            if(!Application.isPlaying)
            {
                var layer = 1;
                var animator = transform.GetComponent<Animator>();
                var weight = animator.GetLayerWeight(layer);
                animator.SetLayerWeight(layer, 0);
                //var stateInfo = animator.runtimeAnimatorController.animationClips
                

                //StartCoroutine(BakeDataPost(animator, layer, weight, onComplete));
                //BakeDataPost(animator, layer, weight, onComplete);
                var t_clip = GetAnimationClip(animator, "0_T-Pose");
                if(t_clip != null)
                {
                    t_clip.SampleAnimation(gameObject, 0);
                }

                BakeDataPost(animator);
            }
        }

        private void BakeDataPost(Animator animator)
        {
            var enumsAll = System.Enum.GetValues(typeof(EFKType));
            var eFKTypes = new EFKType[enumsAll.Length];
            enumsAll.CopyTo(eFKTypes, 0);
            rotationCorrects = FKPoseModelRotateCorrectsGetter.GetCorrects(animator, eFKTypes);
        }

        private AnimationClip GetAnimationClip(Animator animator, string clipName)
        {
            foreach(var clip in animator.runtimeAnimatorController.animationClips)
            {
                if(clip.name == clipName)
                {
                    return clip;
                }
            }
            return null;
        }
    }
}