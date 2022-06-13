using UnityEngine;
using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;

namespace FK
{
    public class FKPoseModel : MonoBehaviour, IFKPoseModel
    {
        [SerializeField] private bool pauseFix;
        [SerializeField] private Quaternion[] rotationCorrects;

        private Animator anim;
        private FKSkeleton skeleton;
        private IMotionDataModel motionDataModel;
        
        [SerializeField] private EFKType[] activeEFKTypes;
        private FKJointPoint[] jointPoints;
        private static readonly EFKType[] defaultEFKTypes = {
            EFKType.Head,
            EFKType.RHip,
            EFKType.LHip,
            EFKType.Neck,
            EFKType.RShoulder,
            EFKType.LShoulder,
            EFKType.RArm,
            EFKType.RWrist,
            EFKType.RHand,
            EFKType.LArm,
            EFKType.LWrist,
            EFKType.LHand,
            EFKType.RKnee,
            EFKType.RAnkle,
            EFKType.LKnee,
            EFKType.LAnkle
        };

        public void Initialize()
        {
            anim = gameObject.GetComponent<Animator>();

            InitJointPoint();
            InitMotionDataModel();
            TryInitEFKTypes();
            InitCorrects(anim);
        }

        public void SetActiveEFKTypes(params EFKType[] eFKTypes)
        {
            this.activeEFKTypes = eFKTypes;
        }

        public void SetEnable(bool active)
        {
            this.enabled = active;
        }

        public bool IsEnabled()
        {
            return this.enabled;
        }

        public void LateUpdate()
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

            SetJointPoint(EFKType.RShoulder, anim);
            SetJointPoint(EFKType.RArm, anim);
            SetJointPoint(EFKType.RWrist, anim);
            SetJointPoint(EFKType.RHand, anim);

            SetJointPoint(EFKType.LShoulder, anim);
            SetJointPoint(EFKType.LArm, anim);
            SetJointPoint(EFKType.LWrist, anim);
            SetJointPoint(EFKType.LHand, anim);

            SetJointPoint(EFKType.Head, anim);
            SetJointPoint(EFKType.Neck, anim);
            SetJointPoint(EFKType.CenterHip, anim);

            SetJointPoint(EFKType.RHip, anim);
            SetJointPoint(EFKType.RKnee, anim);
            SetJointPoint(EFKType.RAnkle, anim);
            SetJointPoint(EFKType.RFoot, anim);

            SetJointPoint(EFKType.LHip, anim);
            SetJointPoint(EFKType.LKnee, anim);
            SetJointPoint(EFKType.LAnkle, anim);
            SetJointPoint(EFKType.LFoot, anim);
        }

        private void SetJointPoint(EFKType eFKType, Animator animator)
        {
            var bodyBone = FKHumanBodyBonesToEFKTypesMapper.GetHumanBodyBone(eFKType);
            jointPoints[eFKType.Int()].Transform = animator.GetBoneTransform(bodyBone);
        }

        public void PoseUpdate()
        {
            if(motionDataModel == null) return;

            var fitting = motionDataModel.GetFitting();
            if(fitting == null || fitting.rotation == null) return;

            if(activeEFKTypes != null)
            {
                var eulerAgY = anim.transform.eulerAngles.y;
                for(int i = 0; i < activeEFKTypes.Length; i++)
                {
                    var index = activeEFKTypes[i].Int();
                    
                    if(index > 0 && index < jointPoints.Length && index < fitting.rotation.Count)
                    {
                        jointPoints[index].Transform.rotation = Quaternion.Euler(0, 180, 0) * anim.transform.rotation * fitting.rotation[index].Rotation();

                        if(!pauseFix && rotationCorrects != null && index < rotationCorrects.Length)
                        {
                            jointPoints[index].Transform.rotation = jointPoints[index].Transform.rotation * rotationCorrects[index];
                        }
                    }
                }
            }
        }

        private void TryInitEFKTypes()
        {
            if(activeEFKTypes == null)
            {
                SetActiveEFKTypes(defaultEFKTypes);
            }
        }

        public void ShowSkeleton()
        {
            if(skeleton == null)
            {
                skeleton = gameObject.AddComponent<FKSkeleton>();
            }
        }

        private void InitCorrects(Animator animator)
        {
            InitCorrects(animator, defaultEFKTypes);
        }

        private void InitCorrects(Animator animator, params EFKType[] eFKTypes)
        {
            var totalTypes = System.Enum.GetNames(typeof(EFKType));
            rotationCorrects = new Quaternion[totalTypes.Length];

            foreach(var eFKType in eFKTypes)
            {
                InitCorrect(animator, eFKType);
            }
        }

        private void InitCorrect(Animator animator, EFKType eFKType)
        {
            var eulerAgY = animator.transform.eulerAngles.y;
            var boneType = FKHumanBodyBonesToEFKTypesMapper.GetHumanBodyBone(eFKType);
            var boneTran = animator.GetBoneTransform(boneType);
            var stanAngs = FKStandardPoseAnglesContainer.GetPosAngles(eFKType);
            var stanRota = Quaternion.Euler(stanAngs);
            var quaterni = Quaternion.Inverse(stanRota) * (Quaternion.Euler(0, -eulerAgY, 0) * boneTran.rotation);
            rotationCorrects[eFKType.Int()] = quaterni;
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