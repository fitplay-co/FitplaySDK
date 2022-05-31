using System;
using System.Collections.Generic;
using UnityEngine;
using MotionCaptureBasic.Interface;

public abstract class ActionReconArm : IActionRecon
{
    protected bool isLeft;
    protected bool lastState;
    
    private List<Vector3> keyPoints;
    private Action<ActionId> onAction;

    public ActionReconArm(bool isLeft, Action<ActionId> onAction)
    {
        this.isLeft = isLeft;
        this.onAction = onAction;
    }

    public void OnUpdate(List<Vector3> keyPoints)
    {
        this.keyPoints = keyPoints;
        if(this.keyPoints != null)
        {
            CheckExtensionHorizon();
            CheckExtensionVerticle();
        }
    }

    protected virtual void CheckExtensionHorizon()
    {
    }

    protected virtual void CheckExtensionVerticle()
    {
    }

    protected void SendEvent(bool active)
    {
        if(onAction != null)
        {
            if(active)
            {
                var actionId = isLeft ? GetActionIdLeft() : GetActionIdRight();
                onAction(actionId);
            }
            else
            {
                onAction(ActionId.None);
            }
        }
    }

    protected float GetForearmAngle()
    {
        var pointHand = GetPointHand();
        var pointElbow = GetPointElbow();
        var pointShoulder = GetPointShoulder();

        var elbowToHand = pointHand - pointElbow;
        var sholderToElbow = pointElbow - pointShoulder;

        //Debug.DrawLine(pointHand, pointElbow, Color.red, 0.1f);
        //Debug.DrawLine(pointElbow, pointShoulder, Color.yellow, 0.1f);

        return Vector3.Angle(elbowToHand, sholderToElbow);
    }

    protected float GetUpperarmAngle()
    {
        /* var shoulderLeft = GetKeyPoint(GameKeyPointsType.LeftShoulder);
        var shoulderRight = GetKeyPoint(GameKeyPointsType.RightShoulder);
        var directHorizon = isLeft ?  shoulderLeft - shoulderRight : -shoulderLeft + shoulderRight; */
        var shoulderToElbow = GetPointElbow() - GetPointShoulder();

        return Vector3.Angle(shoulderToElbow, Vector3.up);
    }

    public void PrintAngle()
    {
        var shoulderToElbow = GetPointElbow() - GetPointShoulder();
        Debug.Log("angle " + Vector3.Angle(shoulderToElbow, Vector3.up));
    }

    protected Vector3 GetPointHand()
    {
        return GetKeyPoint(isLeft ? GameKeyPointsType.LeftHand : GameKeyPointsType.RightHand);
    }

    protected Vector3 GetPointElbow()
    {
        return GetKeyPoint(isLeft ? GameKeyPointsType.LeftElbow : GameKeyPointsType.RightElbow);
    }

    protected Vector3 GetPointShoulder()
    {
        return GetKeyPoint(isLeft ? GameKeyPointsType.LeftShoulder : GameKeyPointsType.RightShoulder);
    }

    private Vector3 GetKeyPoint(GameKeyPointsType pointsType)
    {
        return keyPoints[(int)pointsType];
    }

    protected abstract ActionId GetActionIdLeft();
    protected abstract ActionId GetActionIdRight();
}