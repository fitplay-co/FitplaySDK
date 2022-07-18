using System.Collections.Generic;
using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Scripts
{
    public class FKSkeleton : MonoBehaviour
    {
        [SerializeField] private Material SkeletonMaterial;

        private IMotionDataModel motionDataModel;
        private List<FKSkeletonNode> skeletonList;
        
        [SerializeField] private float SkeletonX = -1;
        [SerializeField] private float SkeletonY = 1;
        [SerializeField] private float SkeletonZ = 0;
        [SerializeField] private float SkeletonScale = 1;

        private EFKType[] eFKTypeAnchors;

        private void Start()
        {
            InitEFKAnchors();
            InitSkeletons();
            InitMotionDataModel();
        }

        public void Update()
        {
            var fitting = motionDataModel.GetFitting();
            if(fitting == null) return;

            UpdataSkeletons(fitting);
        }

        private void ResetSkeletons(params EFKType[] eFKTypes)
        {
            if(skeletonList == null)
            {
                skeletonList = new List<FKSkeletonNode>();
            }
            else
            {
                skeletonList.Clear();
            }

            foreach(var efk in eFKTypes)
            {
                AddSkeleton(efk);
            }
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

        private void InitEFKAnchors()
        {
            var efkTypes = System.Enum.GetNames(typeof(EFKType));
            eFKTypeAnchors = new EFKType[efkTypes.Length];

            // Right Arm
            SetEFKTypeAnchor(EFKType.RShoulder, EFKType.RArm);
            SetEFKTypeAnchor(EFKType.RArm, EFKType.RWrist);
            SetEFKTypeAnchor(EFKType.RWrist, EFKType.RHand);

            // Left Arm
            SetEFKTypeAnchor(EFKType.LShoulder, EFKType.LArm);
            SetEFKTypeAnchor(EFKType.LArm, EFKType.LWrist);
            SetEFKTypeAnchor(EFKType.LWrist, EFKType.LHand);

            // Right Leg
            SetEFKTypeAnchor(EFKType.RHip, EFKType.RKnee);
            SetEFKTypeAnchor(EFKType.RKnee, EFKType.RAnkle);
            SetEFKTypeAnchor(EFKType.RAnkle, EFKType.RFoot);

            // Left Leg
            SetEFKTypeAnchor(EFKType.LHip, EFKType.LKnee);
            SetEFKTypeAnchor(EFKType.LKnee, EFKType.LAnkle);
            SetEFKTypeAnchor(EFKType.LAnkle, EFKType.LFoot);
            
            // etc
            SetEFKTypeAnchor(EFKType.Head, EFKType.Neck);
            SetEFKTypeAnchor(EFKType.Neck, EFKType.RShoulder);
            SetEFKTypeAnchor(EFKType.Neck, EFKType.LShoulder);
            SetEFKTypeAnchor(EFKType.Neck, EFKType.CenterHip);
            SetEFKTypeAnchor(EFKType.CenterHip, EFKType.RHip);
            SetEFKTypeAnchor(EFKType.CenterHip, EFKType.LHip);
        }

        private void SetEFKTypeAnchor(EFKType key, EFKType value)
        {
            eFKTypeAnchors[(int)key] = value;
        }

        private EFKType GetEFKTypeAnchor(EFKType key)
        {
            return eFKTypeAnchors[(int)key];
        }

        private void InitSkeletons()
        {
            if(skeletonList == null)
            {
                ResetSkeletons(
                    EFKType.RShoulder,
                    EFKType.RArm,
                    EFKType.RWrist,
                    EFKType.LShoulder,
                    EFKType.LArm,
                    EFKType.LWrist,
                    EFKType.RHip,
                    EFKType.RKnee,
                    EFKType.RAnkle,
                    EFKType.LHip,
                    EFKType.LKnee,
                    EFKType.LAnkle,
                    EFKType.Head,
                    EFKType.Neck,
                    EFKType.CenterHip
                );
            }
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

        private void AddSkeleton(EFKType s)
        {
            AddSkeleton(s, GetEFKTypeAnchor(s));
        }
    }
}

