using System.Collections.Generic;
using UnityEngine;
using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;

namespace FK
{
    public class FKSkeleton : MonoBehaviour
    {
        //火柴人
        [SerializeField] private Material SkeletonMaterial;

        private IMotionDataModel motionDataModel;
        private List<FKSkeletonNode> skeletonList = new List< FKSkeletonNode>();
        
        [SerializeField] private float SkeletonX = -1;
        [SerializeField] private float SkeletonY = 1;
        [SerializeField] private float SkeletonZ = 0;
        [SerializeField] private float SkeletonScale = 1;

        private void Start()
        {
            InitSkeletons();
            InitMotionDataModel();
        }

        public void Update()
        {
            var fitting = motionDataModel.GetFitting();
            if(fitting == null) return;

            UpdataSkeletons(fitting);
        }

        private void UpdataSkeletons(Fitting fitting)
        {
            var item = new FittingPositionItem();
            var centerHipPos = (fitting.keypoints3D[EFKType.LHip.Int()].Position() + fitting.keypoints3D[EFKType.RHip.Int()].Position())* 0.5f;
            item.x = centerHipPos.x;
            item.x = centerHipPos.y;
            item.x = centerHipPos.z;
            fitting.keypoints3D.Add(item);
            foreach (var sk in skeletonList)
            {
                var s = sk.start;
                var e = sk.end;

                var startX = fitting.keypoints3D[s.Int()].x;
                var startY = fitting.keypoints3D[s.Int()].y;
                var startZ = fitting.keypoints3D[s.Int()].z;
                
                var endX = fitting.keypoints3D[e.Int()].x;
                var endY = fitting.keypoints3D[e.Int()].y;
                var endZ = fitting.keypoints3D[e.Int()].z;
                sk.Line.SetPosition(0, new Vector3(startX * SkeletonScale * SkeletonX, startY * SkeletonScale * SkeletonY, startZ * SkeletonScale * SkeletonZ));
                sk.Line.SetPosition(1, new Vector3(endX * SkeletonScale * SkeletonX, endY * SkeletonScale * SkeletonY, endZ * SkeletonScale * SkeletonZ));
            }
            
        }

        private void InitMotionDataModel()
        {
            motionDataModel = MotionDataModelHttp.GetInstance();
        }

        private void InitSkeletons()
        {
            // Right Arm
            AddSkeleton(EFKType.RShoulder, EFKType.RArm);
            AddSkeleton(EFKType.RArm, EFKType.RWrist);
            AddSkeleton(EFKType.RWrist, EFKType.RHand);

            // Left Arm
            AddSkeleton(EFKType.LShoulder, EFKType.LArm);
            AddSkeleton(EFKType.LArm, EFKType.LWrist);
            AddSkeleton(EFKType.LWrist, EFKType.LHand);
            
            // Right Leg
            AddSkeleton(EFKType.RHip, EFKType.RKnee);
            AddSkeleton(EFKType.RKnee, EFKType.RAnkle);
            AddSkeleton(EFKType.RAnkle, EFKType.RFoot);
            
            // Left Leg
            AddSkeleton(EFKType.LHip, EFKType.LKnee);
            AddSkeleton(EFKType.LKnee, EFKType.LAnkle);
            AddSkeleton(EFKType.LAnkle, EFKType.LFoot);
            
            // etc
            AddSkeleton(EFKType.Head, EFKType.Neck);
            AddSkeleton(EFKType.Neck, EFKType.RShoulder);
            AddSkeleton(EFKType.Neck, EFKType.LShoulder);
            AddSkeleton(EFKType.Neck, EFKType.CenterHip);
            AddSkeleton(EFKType.CenterHip, EFKType.RHip);
            AddSkeleton(EFKType.CenterHip, EFKType.LHip);
        }

        private void AddSkeleton(EFKType s, EFKType e)
        {
            var sk = new FKSkeletonNode()
            {
                LineObject = new GameObject("Line"),
                start = s,
                end = e,
            };

            sk.Line = sk.LineObject.AddComponent<LineRenderer>();
            sk.Line.startWidth = 0.04f;
            sk.Line.endWidth = 0.01f;
            
            // define the number of vertex
            sk.Line.positionCount = 2;
            sk.Line.material = SkeletonMaterial;

            skeletonList.Add(sk);
        }
    }
}

