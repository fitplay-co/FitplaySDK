using System;
using System.Collections;
using System.Collections.Generic;
using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class FKAnimatorJoints : MonoBehaviour
    {
        public bool fkActive = true;
        public bool mirror = false;
        public bool applyHipRotation = true;

        public Quaternion hipModelSkeletonAdapter = Quaternion.Euler(68, 90, 180);
        public Quaternion LegModelSkeletonAdapter = Quaternion.Euler(90, 270, 0);
        public Quaternion UpperArmModelSkeletonAdapter = Quaternion.Euler(270, 90, 0);
        public Quaternion LowArmModelSkeletonAdapter = Quaternion.Euler(270, 90, 0);
        public Quaternion SpineModelSkeletonAdapter = Quaternion.Euler(270, 90, 0);

        private Animator animator;
        private List<FittingRotationItem> cachedFrameFitting;
        private List<FittingRotationItem> globalCachedFitting;

        private static readonly FKJpintMap[] DefaultFKJpintTypes =
        {
            FKJpintMap.LHip,
            FKJpintMap.Spine,
            FKJpintMap.LShoulder,
            FKJpintMap.LUpArm,
            FKJpintMap.LLowArm,
            FKJpintMap.RShoulder,
            FKJpintMap.RUpArm,
            FKJpintMap.RLowArm,
            FKJpintMap.LUpLeg,
            FKJpintMap.LLowLeg,
            FKJpintMap.RUpLeg,
            FKJpintMap.RLowLeg
        };
        [SerializeField] private FKJpintMap[] activateFKJpintTypes;

        private Dictionary<FKJpintMap, BonesData> FKJpintBoneMap;

        private void Start () {
            //calculate bone length to fit FK calculation 
            animator = GetComponent<Animator>();

            Initialization();
        }

        private void Initialization()
        {
            if (FKJpintBoneMap == null)
            {
                FKJpintBoneMap = new Dictionary<FKJpintMap, BonesData>();
            }

            FKJpintBoneMap.Add(FKJpintMap.LHip, 
                new BonesData
                {
                    boneType = HumanBodyBones.Hips, 
                    boneTransform = animator.GetBoneTransform(HumanBodyBones.Hips),
                    adapter = hipModelSkeletonAdapter
                });
            FKJpintBoneMap.Add(FKJpintMap.Spine, 
                new BonesData
                {
                    boneType = HumanBodyBones.Spine, 
                    boneTransform = animator.GetBoneTransform(HumanBodyBones.Spine),
                    adapter = SpineModelSkeletonAdapter
                });
            FKJpintBoneMap.Add(FKJpintMap.LShoulder, 
                new BonesData
                {
                    boneType = HumanBodyBones.LeftShoulder, 
                    boneTransform = animator.GetBoneTransform(HumanBodyBones.LeftShoulder),
                    adapter = LegModelSkeletonAdapter
                });
            FKJpintBoneMap.Add(FKJpintMap.LUpArm, 
                new BonesData
                {
                    boneType = HumanBodyBones.LeftUpperArm, 
                    boneTransform = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm),
                    adapter = UpperArmModelSkeletonAdapter
                });
            FKJpintBoneMap.Add(FKJpintMap.LLowArm, 
                new BonesData
                {
                    boneType = HumanBodyBones.LeftLowerArm, 
                    boneTransform = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm),
                    adapter = LowArmModelSkeletonAdapter
                });
            FKJpintBoneMap.Add(FKJpintMap.RShoulder, 
                new BonesData
                {
                    boneType = HumanBodyBones.RightShoulder, 
                    boneTransform = animator.GetBoneTransform(HumanBodyBones.RightShoulder),
                    adapter = LegModelSkeletonAdapter
                });
            FKJpintBoneMap.Add(FKJpintMap.RUpArm, 
                new BonesData
                {
                    boneType = HumanBodyBones.RightUpperArm, 
                    boneTransform = animator.GetBoneTransform(HumanBodyBones.RightUpperArm),
                    adapter = UpperArmModelSkeletonAdapter,
                });
            FKJpintBoneMap.Add(FKJpintMap.RLowArm, 
                new BonesData
                {
                    boneType = HumanBodyBones.RightLowerArm, 
                    boneTransform = animator.GetBoneTransform(HumanBodyBones.RightLowerArm),
                    adapter = LowArmModelSkeletonAdapter
                });
            FKJpintBoneMap.Add(FKJpintMap.LUpLeg, 
                new BonesData
                {
                    boneType = HumanBodyBones.LeftUpperLeg, 
                    boneTransform = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg),
                    adapter = LegModelSkeletonAdapter
                });
            FKJpintBoneMap.Add(FKJpintMap.LLowLeg, 
                new BonesData
                {
                    boneType = HumanBodyBones.LeftLowerLeg, 
                    boneTransform = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg),
                    adapter = LegModelSkeletonAdapter
                });
            FKJpintBoneMap.Add(FKJpintMap.RUpLeg, 
                new BonesData
                {
                    boneType = HumanBodyBones.RightUpperLeg, 
                    boneTransform = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg),
                    adapter = LegModelSkeletonAdapter
                });
            FKJpintBoneMap.Add(FKJpintMap.RLowLeg, 
                new BonesData
                {
                    boneType = HumanBodyBones.RightLowerLeg, 
                    boneTransform = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg),
                    adapter = LegModelSkeletonAdapter
                });
        }

        public void LateUpdate() {
            // hipCenter.rotation = Quaternion.identity;
            // Debug.Log("after local: " + hipCenter.localRotation.x + " " + hipCenter.localRotation.y + " " + hipCenter.localRotation.z + " " +  hipCenter.localRotation.w);
            // Debug.Log("after rot: " + hipCenter.rotation.x + " " + hipCenter.rotation.y + " " + hipCenter.rotation.z + " " +  hipCenter.rotation.w);
        }

        public void SetFullFKJpintTypes()
        {
            activateFKJpintTypes = DefaultFKJpintTypes;
        }

        public void SetActiveFKJpintTypes(FKJpintMap[] jpintTypes)
        {
            activateFKJpintTypes = jpintTypes;
        }
        
        public void SetEnable(bool active)
        {
            this.enabled = active;
        }

        public bool IsEnabled()
        {
            return this.enabled;
        }

        public void UpdateFkInfo(Fitting fitting){
            if (mirror)  { 
                cachedFrameFitting = fitting.mirrorLocalRotation;
                globalCachedFitting = fitting.mirrorRotation;
            } else {
                cachedFrameFitting = fitting.localRotation;
                globalCachedFitting = fitting.rotation;
            }
            // Debug.Log("update ! ");
            // Debug.Log(fitting.rotation[0].w + " "  + fitting.rotation[0].x + " " + fitting.rotation[0].y + " " +fitting.rotation[0].z);
        }

        //a callback for calculating IK
        private void OnAnimatorIK(int layerIndex)
        {
            if(animator != null) {
                //if the IK is active, set the position and rotation directly to the goal.
                if(fkActive && cachedFrameFitting != null) {
                    // Set the look target position, if one has been assigned
                    foreach (var fkJpintType in activateFKJpintTypes)
                    {
                        var boneData = FKJpintBoneMap[fkJpintType];
                        if (fkJpintType == FKJpintMap.LHip)
                        {
                            if (applyHipRotation)
                            {
                                var localRotation = globalCachedFitting[fkJpintType.Int()].Rotation() *
                                                    boneData.adapter;
                                animator.SetBoneLocalRotation(FKJpintBoneMap[fkJpintType].boneType, localRotation);
                            }
                        }
                        else
                        {
                            var localRotation = boneData.boneTransform.localRotation *
                                                adaptQuat(cachedFrameFitting[fkJpintType.Int()].Rotation(),
                                                    boneData.adapter);
                            animator.SetBoneLocalRotation(FKJpintBoneMap[fkJpintType].boneType, localRotation);
                        }
                    }
                }
            }
        }  

        Quaternion adaptQuat(Quaternion source, Quaternion adapter){
            return Quaternion.Inverse(adapter) * source * adapter;
        }
    }

    public enum FKJpintMap {
        Spine = 0,
        Neck = 1,
        LShoulder = 2,
        RShoulder = 3,
        LUpArm = 4,
        RUpArm = 5,
        LLowArm = 6,
        RLowArm = 7,
        LHand = 8,
        RHand = 9,
        LHip = 10,
        RHip = 11,
        LUpLeg = 12 ,
        RUpLeg = 13,
        LLowLeg = 14,
        RLowLeg =15,
        LFoot = 16,
        RFoot = 17,
        Root = 18,
        
        Count = Root
    }

    public static class EnumExtend {
        public static int Int(this FKJpintMap i) {
            return (int)i;
        }
    }

    [Serializable]
    public class BonesData
    {
        public Transform boneTransform;
        public HumanBodyBones boneType;
        public Quaternion adapter;
    }
}