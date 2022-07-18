using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Scripts
{
    public static class FKStandardPoseAnglesContainer
    {
        private static Vector3[] posAngles;
        private static Vector3[] posForwards;

        public static Vector3 GetPosAngles(EFKType eFKType)
        {
            TryInit();
            return posAngles[eFKType.Int()];
        }

        private static void TryInit()
        {
            if(posAngles == null)
            {
                var eFKTypes = System.Enum.GetNames(typeof(EFKType));
                posAngles = new Vector3[eFKTypes.Length];

                AddEulerAngles(EFKType.RShoulder,   new Vector3(270,        180,        0));
                AddEulerAngles(EFKType.RArm,        new Vector3(270.3f,     89.8f,      90.1f));
                AddEulerAngles(EFKType.RWrist,      new Vector3(270.4f,     91.9f,      88.0f));
                AddEulerAngles(EFKType.RHand,       new Vector3(280.2f,     91.1f,      88.9f));

                AddEulerAngles(EFKType.LShoulder,   new Vector3(90,         0,          0));
                AddEulerAngles(EFKType.LArm,        new Vector3(89.7f,      90.2f,      90.1f));
                AddEulerAngles(EFKType.LWrist,      new Vector3(89.6f,      88.2f,      88.1f));
                AddEulerAngles(EFKType.LHand,       new Vector3(77.3f,      350.7f,     350.4f));

                AddEulerAngles(EFKType.Head,        new Vector3(0,          270,        267.4f));
                AddEulerAngles(EFKType.Neck,        new Vector3(0,          270,        270.7f));
                AddEulerAngles(EFKType.CenterHip,   new Vector3(0.2f,       270,        270));

                AddEulerAngles(EFKType.RHip,        new Vector3(0.2f,       270,        270));
                AddEulerAngles(EFKType.RKnee,       new Vector3(0,          90,         91.3f));
                AddEulerAngles(EFKType.RAnkle,      new Vector3(0,          90,         93.2f));
                AddEulerAngles(EFKType.RFoot,       new Vector3(0,          90,         62.5f));

                AddEulerAngles(EFKType.LHip,        new Vector3(0.2f,       270,        270));
                AddEulerAngles(EFKType.LKnee,       new Vector3(357.9f,     90,         91.3f));
                AddEulerAngles(EFKType.LAnkle,      new Vector3(357.9f,     90,         93.2f));
                AddEulerAngles(EFKType.LFoot,       new Vector3(357.9f,     90,         62.5f));
            }

            if(posForwards == null)
            {
                var eFKTypes = System.Enum.GetNames(typeof(EFKType));
                posForwards = new Vector3[eFKTypes.Length];
            }
        }

        private static void AddEulerAngles(EFKType eFKType, Vector3 angles)
        {
            posAngles[(int)eFKType] = angles;
        }

        private static void AddBoneForward(EFKType eFKType, Vector3 forward)
        {
            posForwards[(int)eFKType] = forward;
        }
    }
}