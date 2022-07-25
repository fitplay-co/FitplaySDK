using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;

[RequireComponent(typeof(Animator))]

public class FKAnimatorJoints : MonoBehaviour
{
    protected Animator animator;
    public bool fkActive = true;
    public bool mirror = false;

    public Quaternion LegModelSkeletonAdapter = Quaternion.Euler(90, 270 , 0);
    public Quaternion UpperArmModelSkeletonAdapter = Quaternion.Euler(270, 90 , 0);
    public Quaternion LowArmModelSkeletonAdapter = Quaternion.Euler(270, 90 , 0);
    public Quaternion SpineModelSkeletonAdapter = Quaternion.Euler(270, 90 , 0);

    Transform hipCenter;
    Transform spine;
    Transform lShoulder;
    Transform lUpperArm;
    Transform llowerArm;
    Transform rShoulder;
    Transform rUpperArm;
    Transform rlowerArm;
    Transform lUpperLeg;
    Transform lLowerLeg;
    Transform rUpperLeg;
    Transform rLowerLeg;
    List<FittingRotationItem> cachedFrameFitting;

    void Avake() { 
    }

    void Start () {
        //calculate bone length to fit FK calculation 
        animator = GetComponent<Animator>();
        
        hipCenter = animator.GetBoneTransform(HumanBodyBones.Hips);
        spine = animator.GetBoneTransform(HumanBodyBones.Spine); 

        //upper body 
        lShoulder = animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
        lUpperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        llowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        rShoulder = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
        rUpperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
        rlowerArm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);

        //lower body
        lUpperLeg = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        lLowerLeg = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        rUpperLeg = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        rLowerLeg = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
    }

    public void LateUpdate() {
        // hipCenter.rotation = Quaternion.identity;
        // Debug.Log("after local: " + hipCenter.localRotation.x + " " + hipCenter.localRotation.y + " " + hipCenter.localRotation.z + " " +  hipCenter.localRotation.w);
        // Debug.Log("after rot: " + hipCenter.rotation.x + " " + hipCenter.rotation.y + " " + hipCenter.rotation.z + " " +  hipCenter.rotation.w);
    }

    public void updateFkInfo(Fitting fitting){
        if (mirror)  { 
            cachedFrameFitting = fitting.mirrorLocalRotation;
        } else {
            cachedFrameFitting = fitting.localRotation;
        }
        // Debug.Log("update ! ");
        // Debug.Log(fitting.rotation[0].w + " "  + fitting.rotation[0].x + " " + fitting.rotation[0].y + " " +fitting.rotation[0].z);
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if(animator) {
            //if the IK is active, set the position and rotation directly to the goal.
            if(fkActive && cachedFrameFitting != null) {
                // Set the look target position, if one has been assigned
                // animator.SetBoneLocalRotation(HumanBodyBones.Hips, hipCenter.localRotation);
                animator.SetBoneLocalRotation(HumanBodyBones.Spine, spine.localRotation * adaptQuat(cachedFrameFitting[FKJpintMap.Spine.Int()].Rotation(), SpineModelSkeletonAdapter));
                animator.SetBoneLocalRotation(HumanBodyBones.LeftShoulder, lShoulder.localRotation * adaptQuat(cachedFrameFitting[FKJpintMap.LShoulder.Int()].Rotation(), LegModelSkeletonAdapter));
                animator.SetBoneLocalRotation(HumanBodyBones.LeftUpperArm, lUpperArm.localRotation * adaptQuat(cachedFrameFitting[FKJpintMap.LUpArm.Int()].Rotation(), UpperArmModelSkeletonAdapter));
                animator.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, llowerArm.localRotation * adaptQuat(cachedFrameFitting[FKJpintMap.LLowArm.Int()].Rotation(), LowArmModelSkeletonAdapter));
                animator.SetBoneLocalRotation(HumanBodyBones.RightShoulder, rShoulder.localRotation * adaptQuat(cachedFrameFitting[FKJpintMap.RShoulder.Int()].Rotation(), LegModelSkeletonAdapter));
                animator.SetBoneLocalRotation(HumanBodyBones.RightUpperArm, rUpperArm.localRotation * adaptQuat(cachedFrameFitting[FKJpintMap.RUpArm.Int()].Rotation(), UpperArmModelSkeletonAdapter));
                animator.SetBoneLocalRotation(HumanBodyBones.RightLowerArm, rlowerArm.localRotation * adaptQuat(cachedFrameFitting[FKJpintMap.RLowArm.Int()].Rotation(), LowArmModelSkeletonAdapter));
                animator.SetBoneLocalRotation(HumanBodyBones.LeftUpperLeg, lUpperLeg.localRotation * adaptQuat(cachedFrameFitting[FKJpintMap.LUpLeg.Int()].Rotation(), LegModelSkeletonAdapter));
                animator.SetBoneLocalRotation(HumanBodyBones.LeftLowerLeg, lLowerLeg.localRotation * adaptQuat(cachedFrameFitting[FKJpintMap.LLowLeg.Int()].Rotation(), LegModelSkeletonAdapter));
                animator.SetBoneLocalRotation(HumanBodyBones.RightUpperLeg, rUpperLeg.localRotation * adaptQuat(cachedFrameFitting[FKJpintMap.RUpLeg.Int()].Rotation(), LegModelSkeletonAdapter));
                animator.SetBoneLocalRotation(HumanBodyBones.RightLowerLeg, rLowerLeg.localRotation * adaptQuat(cachedFrameFitting[FKJpintMap.RLowLeg.Int()].Rotation(), LegModelSkeletonAdapter));       
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