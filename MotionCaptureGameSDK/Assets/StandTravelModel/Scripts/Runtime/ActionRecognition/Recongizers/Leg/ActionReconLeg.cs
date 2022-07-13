using System;
using MotionCaptureBasic.Interface;
using StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconComponents;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.Recongizers.Leg
{
    public class ActionReconLeg : ActionReconLimb
    {
        protected const float angleUpKneeMin = 45;
        protected const float angleUpKneeMax = 180;

        protected const float angleUpHipMin = 0;
        protected const float angleUpHipMax = 145;

        private bool isUp;
        private ActionReconCompAngle angleKnee;
        private ActionReconCompAngle angleHip;

        public ActionReconLeg(bool isLeft, bool isUp, Vector2 anglesKnee, Vector2 anglesHip, Action<ActionId> onAction) : base(isLeft, onAction)
        {
            this.isUp = isUp;
            /* this.angleKnee = new ActionReconCompAngleGradient(anglesKnee.x, anglesKnee.y, isUp, CreateAngleGetter(isLeft));
        this.angleHip = new ActionReconCompAngleGradient(anglesHip.x, anglesHip.y, !isUp, CreateAngleGetterWithAngle(isLeft)); */
            this.angleKnee = new ActionReconCompAngle(anglesKnee.x, anglesKnee.y, CreateAngleGetter(isLeft));
            this.angleHip = new ActionReconCompAngle(anglesHip.x, anglesHip.y, CreateAngleGetterWithAngle(isLeft));
    
            AddReconComp(angleKnee);
            AddReconComp(angleHip);
        }

        public static ReconCompAngleGetter CreateAngleGetter(bool isLeft)
        {
            return new ReconCompAngleGetter(
                GetPointsTypeFoot(isLeft),
                GetPointsTypeKnee(isLeft),
                GetPointsTypeHip(isLeft)
            );
        }

        public static ReconCompAngleGetterWithDirect CreateAngleGetterWithAngle(bool isLeft)
        {
            return new ReconCompAngleGetterWithDirect(
                GetPointsTypeFoot(isLeft),
                GetPointsTypeKnee(isLeft),
                GetPointsTypeHip(isLeft),
                Vector3.up
            );
        }

        protected override ActionId GetActionIdLeft()
        {
            return isUp ? ActionId.LegUpLeft : ActionId.LegDownLeft;
        }

        protected override ActionId GetActionIdRight()
        {
            return isUp ? ActionId.LegUpRight : ActionId.LegDownRight;
        }

        private static GameKeyPointsType GetPointsTypeFoot(bool isLeft)
        {
            return isLeft ? GameKeyPointsType.LeftFoot : GameKeyPointsType.RightFoot;
        }

        private static GameKeyPointsType GetPointsTypeKnee(bool isLeft)
        {
            return isLeft ? GameKeyPointsType.LeftKnee : GameKeyPointsType.RightKnee;
        }

        private static GameKeyPointsType GetPointsTypeHip(bool isLeft)
        {
            return isLeft ? GameKeyPointsType.LeftHip : GameKeyPointsType.RightHip;
        }
    }
}