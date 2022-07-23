using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;


    public class JointPoint {
        // Bones
        public Transform Transform;

        public Quaternion InitRotation;
        public Quaternion Inverse;
        public Quaternion InverseRotation;
        
        public JointPoint Child;
        public JointPoint Parent;
    }

    public class TestFk : MonoBehaviour {
        public GameObject ModelObject;
        public Quaternion modelSkeletonAdapter = Quaternion.Euler(90, 270 , 0);

        private Animator anim;

        public bool IsMirror;
        
        private JointPoint[] jointPoints;

        private Quaternion initRotation;

        private void Start()
        {
            InitJointPoint();
            initRotation = ModelObject.transform.rotation;
        }

        private void InitJointPoint()
        {
            jointPoints = new JointPoint[EFKType.Count.Int() ];
            for (var i = 0; i < EFKType.Count.Int(); i++) jointPoints[i] = new JointPoint();

            anim = ModelObject.GetComponent<Animator>();
            
            // Right Arm
            jointPoints[FKJpintMap.RShoulder.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightShoulder);
            jointPoints[FKJpintMap.RUpArm.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
            jointPoints[FKJpintMap.RLowArm.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
            jointPoints[FKJpintMap.RHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightHand);

            // Left Arm
            jointPoints[FKJpintMap.LShoulder.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftShoulder);
            jointPoints[FKJpintMap.LUpArm.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            jointPoints[FKJpintMap.LLowArm.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            jointPoints[FKJpintMap.LHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftHand);

            // etc
            jointPoints[FKJpintMap.Spine.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Spine);
            jointPoints[FKJpintMap.Neck.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);

            // Right Leg
            jointPoints[FKJpintMap.RHip.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Hips);
            jointPoints[FKJpintMap.RUpLeg.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
            jointPoints[FKJpintMap.RLowLeg.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerLeg);
            jointPoints[FKJpintMap.RFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightFoot);

            // Left Leg
            jointPoints[FKJpintMap.LHip.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Hips);
            jointPoints[FKJpintMap.LUpLeg.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
            jointPoints[FKJpintMap.LLowLeg.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
            jointPoints[FKJpintMap.LFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        }
        
        private bool inited = false;
        private Quaternion[] initBone = new Quaternion[18];

        public void updateFkInfo(Fitting fitting, List<Vector3> keypoints3D) {
            if (!inited) {
                inited = true;
                for (var i = 0; i < FKJpintMap.Count.Int() - 1; i++) {
                    initBone[i] = jointPoints[i].Transform.localRotation;
                } 
            }
            if (IsMirror) {
                for (var i = 0; i < EFKType.Count.Int() - 1; i++) {
                    if(i == FKJpintMap.RFoot.Int() || i == FKJpintMap.LFoot.Int() || i == FKJpintMap.Neck.Int()) continue;
                    jointPoints[i].Transform.localRotation = initBone[i] * Quaternion.Inverse(modelSkeletonAdapter) * fitting.mirrorLocalRotation[i].Rotation() * modelSkeletonAdapter;
                } 
            } else {
                for (var i = 0; i < EFKType.Count.Int() - 1; i++) {
                    if(i == FKJpintMap.RFoot.Int() || i == FKJpintMap.LFoot.Int() || i == FKJpintMap.Neck.Int()) continue;
                    jointPoints[i].Transform.localRotation = initBone[i] * Quaternion.Inverse(modelSkeletonAdapter) * fitting.localRotation[i].Rotation() * modelSkeletonAdapter;
                }
            }
            //TODO test here 
            // int j = FKJpintMap.Spine.Int();

            //some mathmatics please consult author ~
            // jointPoints[j].Transform.localRotation = initBone[j] * Quaternion.Inverse(modelSkeletonAdapter) * Quaternion.Euler(90,90,45)* modelSkeletonAdapter;
        }
    }
