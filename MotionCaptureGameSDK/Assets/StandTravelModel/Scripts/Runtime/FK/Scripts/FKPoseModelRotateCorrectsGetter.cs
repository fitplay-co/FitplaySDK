using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Scripts
{
    public static class FKPoseModelRotateCorrectsGetter
    {
        public static Quaternion[] GetCorrects(Animator animator, params EFKType[] eFKTypes)
        {
            var totalTypes = System.Enum.GetNames(typeof(EFKType));
            var rotationCorrects = new Quaternion[totalTypes.Length];

            foreach(var eFKType in eFKTypes)
            {
                GetCorrect(animator, eFKType, rotationCorrects);
            }

            return rotationCorrects;
        }

        private static void GetCorrect(Animator animator, EFKType eFKType, Quaternion[] rotationCorrects)
        {
            var eulerAgY = animator.transform.eulerAngles.y;
            var boneType = FKHumanBodyBonesToEFKTypesMapper.GetHumanBodyBone(eFKType);
            var boneTran = animator.GetBoneTransform(boneType);
            var stanAngs = FKStandardPoseAnglesContainer.GetPosAngles(eFKType);
            var stanRota = Quaternion.Euler(stanAngs);
            var quaterni = Quaternion.Inverse(stanRota) * (Quaternion.Euler(0, -eulerAgY, 0) * boneTran.rotation);
            rotationCorrects[eFKType.Int()] = quaterni;
        }
    }  
}