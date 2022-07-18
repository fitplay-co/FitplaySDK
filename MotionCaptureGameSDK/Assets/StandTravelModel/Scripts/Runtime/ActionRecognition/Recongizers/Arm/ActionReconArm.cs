using System;
using MotionCaptureBasic.Interface;
using StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconComponents;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.Recongizers.Arm
{
    public abstract class ActionReconArm : ActionReconLimb
    {
        private ActionReconCompAngle foreArmComp;
        private ActionReconCompAngle upperArmComp;

        public ActionReconArm(bool isLeft, Vector2 anglesElbow, Vector2 anglesShoulder, Action<ActionId> onAction) : base (isLeft, onAction)
        {
            foreArmComp = new ActionReconCompAngle(
                anglesElbow.x,
                anglesElbow.y,
                CreateAngleGetter(isLeft)
            );

            upperArmComp = new ActionReconCompAngle(
                anglesShoulder.x,
                anglesShoulder.y,
                CreateAngleGetterWithDirect(isLeft)
            );

            AddReconComp(foreArmComp);
            AddReconComp(upperArmComp);
        }

        protected float GetUpperarmAngle()
        {
            return upperArmComp.GetCurAngle();
        }

        private ReconCompAngleGetter CreateAngleGetter(bool isLeft)
        {
            return new ReconCompAngleGetter(GetPointTypeHand(isLeft), GetPointTypeElbow(isLeft), GetPointTypeShoulder(isLeft));
        }

        private ReconCompAngleGetterWithDirect CreateAngleGetterWithDirect(bool isLeft)
        {
            return new ReconCompAngleGetterWithDirect(GetPointTypeHand(isLeft), GetPointTypeElbow(isLeft), GetPointTypeShoulder(isLeft), Vector3.up);
        }

        private GameKeyPointsType GetPointTypeHand(bool isLeft)
        {
            return isLeft ? GameKeyPointsType.LeftHand : GameKeyPointsType.RightHand;
        }

        private GameKeyPointsType GetPointTypeElbow(bool isLeft)
        {
            return isLeft ? GameKeyPointsType.LeftElbow : GameKeyPointsType.RightElbow;
        }

        private GameKeyPointsType GetPointTypeShoulder(bool isLeft)
        {
            return isLeft ? GameKeyPointsType.LeftShoulder : GameKeyPointsType.RightShoulder;
        }
    }
}