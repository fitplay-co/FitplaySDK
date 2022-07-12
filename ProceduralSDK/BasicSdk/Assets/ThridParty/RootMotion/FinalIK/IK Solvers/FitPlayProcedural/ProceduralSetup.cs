using UnityEngine;

namespace RootMotion.FinalIK.FitPlayProcedural
{
    public class ProceduralSetup
    {

            public float footDistance = 0.3f;

            public float stepThreshold = 0.4f;


            public float angleThreshold = 60f;

     
            public float comAngleMlp = 1f;


            public float maxVelocity = 0.4f;


            public float velocityFactor = 0.4f;

            public float maxLegStretch = 1f;


            public float rootSpeed = 20f;


            public float stepSpeed = 3f;


            public AnimationCurve stepHeight;

            public float maxBodyYOffset = 0.05f;


            public AnimationCurve heelHeight;


            public float relaxLegTwistMinAngle = 20f;


            public float relaxLegTwistSpeed = 400f;

            public InterpolationMode stepInterpolation = InterpolationMode.InOutSine;

            public Vector3 offset;
        
    }
}