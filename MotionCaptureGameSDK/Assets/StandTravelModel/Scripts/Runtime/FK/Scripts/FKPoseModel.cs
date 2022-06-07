using UnityEngine;
using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;

namespace FK
{
    public class FKPoseModel : MonoBehaviour
    {
        public GameObject ModelObject;
        private Animator anim;
        private IMotionDataModel motionDataModel;
        
        private FKJointPoint[] jointPoints;
        private void Start()
        {
            InitJointPoint();
            InitMotionDataModel();
        }

        public void Update()
        {
            if (jointPoints != null)
            {
                PoseUpdate();
            }
        }

        private void InitMotionDataModel()
        {
            motionDataModel = MotionDataModelHttp.GetInstance();
        }

        private void InitJointPoint()
        {
            jointPoints = new FKJointPoint[EFKType.Count.Int() + 1];
            for (var i = 0; i < EFKType.Count.Int() + 1; i++) jointPoints[i] = new FKJointPoint();
            
            anim = ModelObject.GetComponent<Animator>();
            
            // Right Arm
            jointPoints[EFKType.RShoulder.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightShoulder);
            jointPoints[EFKType.RArm.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
            jointPoints[EFKType.RWrist.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
            jointPoints[EFKType.RHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightHand);
            
            // Left Arm
            jointPoints[EFKType.LShoulder.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftShoulder);
            jointPoints[EFKType.LArm.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            jointPoints[EFKType.LWrist.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            jointPoints[EFKType.LHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftHand);
            
            // etc
            jointPoints[EFKType.Head.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Neck);
            jointPoints[EFKType.Neck.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Spine);
            jointPoints[EFKType.CenterHip.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Hips);
            
            // Right Leg
            jointPoints[EFKType.RHip.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Hips);
            jointPoints[EFKType.RKnee.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
            jointPoints[EFKType.RAnkle.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerLeg);
            jointPoints[EFKType.RFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightFoot);
            
            // Left Leg
            jointPoints[EFKType.LHip.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Hips);
            jointPoints[EFKType.LKnee.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
            jointPoints[EFKType.LAnkle.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
            jointPoints[EFKType.LFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        }

        public void PoseUpdate()
        {
            /* var message = ProtoHandler.Instance.BodyMessageBase;
            var fitting = message?.fitting; */
            if(motionDataModel == null) return;

            var fitting = motionDataModel.GetFitting();
            if(fitting == null) return;
            //print("fitting:" + JsonUtility.ToJson(fitting));
            /*
            for (int i = 0; i < EFKType.Count.Int(); i++)
            {
                jointPoints[i].Transform.localRotation = fitting.rotation[i].Rotation() * jointPoints[i].InitRotation;
            //    print($"type:{(EFKType)i} rotation:{fitting.rotation[i].Rotation()}");
            }
            */
           
            /*
            var item = new FittingPositionItem();
            var centerHipPos = (fitting.keypoints3D[EFKType.LHip.Int()].Position() + fitting.keypoints3D[EFKType.RHip.Int()].Position())* 0.5f;
            item.x = centerHipPos.x;
            item.x = centerHipPos.y;
            item.x = centerHipPos.z;

            jointPoints[EFKType.CenterHip.Int()].Transform.position = centerHipPos;
            */
            
            jointPoints[EFKType.RHip.Int()].Transform.rotation = fitting.rotation[EFKType.RHip.Int()].Rotation();
            jointPoints[EFKType.LHip.Int()].Transform.rotation = fitting.rotation[EFKType.LHip.Int()].Rotation();
            
            jointPoints[EFKType.Neck.Int()].Transform.rotation = fitting.rotation[EFKType.Neck.Int()].Rotation();
            
            //jointPoints[EFKType.Head.Int()].Transform.rotation = fitting.rotation[EFKType.Head.Int()].Rotation();
            jointPoints[EFKType.RShoulder.Int()].Transform.rotation = fitting.rotation[EFKType.RShoulder.Int()].Rotation();
            jointPoints[EFKType.LShoulder.Int()].Transform.rotation = fitting.rotation[EFKType.LShoulder.Int()].Rotation();

            jointPoints[EFKType.RArm.Int()].Transform.rotation = fitting.rotation[EFKType.RArm.Int()].Rotation();
            jointPoints[EFKType.RWrist.Int()].Transform.rotation = fitting.rotation[EFKType.RWrist.Int()].Rotation();
            jointPoints[EFKType.RHand.Int()].Transform.rotation = fitting.rotation[EFKType.RHand.Int()].Rotation();
            
            jointPoints[EFKType.LArm.Int()].Transform.rotation = fitting.rotation[EFKType.LArm.Int()].Rotation();
            jointPoints[EFKType.LWrist.Int()].Transform.rotation = fitting.rotation[EFKType.LWrist.Int()].Rotation();
            jointPoints[EFKType.LHand.Int()].Transform.rotation = fitting.rotation[EFKType.LHand.Int()].Rotation();
            
            jointPoints[EFKType.RKnee.Int()].Transform.rotation = fitting.rotation[EFKType.RKnee.Int()].Rotation();
            jointPoints[EFKType.RAnkle.Int()].Transform.rotation = fitting.rotation[EFKType.RAnkle.Int()].Rotation();
            //jointPoints[EFKType.RFoot.Int()].Transform.rotation = fitting.rotation[EFKType.RFoot.Int()].Rotation();
            
            jointPoints[EFKType.LKnee.Int()].Transform.rotation = fitting.rotation[EFKType.LKnee.Int()].Rotation();
            jointPoints[EFKType.LAnkle.Int()].Transform.rotation = fitting.rotation[EFKType.LAnkle.Int()].Rotation();
            //jointPoints[EFKType.LFoot.Int()].Transform.rotation = fitting.rotation[EFKType.LFoot.Int()].Rotation();
        }

        private Vector3 TriangleNormal(Vector3 a, Vector3 b, Vector3 c)
        {
            var d1 = a - b;
            var d2 = a - c;

            var dd = Vector3.Cross(d1, d2);
            dd.Normalize();

            return dd;
        }
        private Quaternion GetInverse(FKJointPoint p1, FKJointPoint p2, Vector3 forward)
        {
            return Quaternion.Inverse(Quaternion.LookRotation(p1.Transform.position - p2.Transform.position, forward));
        }
    }
}