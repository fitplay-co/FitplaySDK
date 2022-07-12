using System.Collections;
using System.Collections.Generic;
using RootMotion;
using UnityEngine;
using UnityEngine.Events;

public class IKProceduralFootstep
{
    public Vector3 characterSpaceOffset;
    public Vector3 position;
    public Quaternion rotation = Quaternion.identity;
    public Quaternion stepToRootRot = Quaternion.identity;
    public bool isStepping => stepProgress < 1f;
    public float stepSpeed = 3f;

    public bool isSupportLeg;
    // public bool relaxFlag;

    public float stepProgress { get; private set; }
    public Vector3 stepFrom;
    public Vector3 stepTo;
    public Quaternion stepFromRot = Quaternion.identity;
    public Quaternion stepToRot = Quaternion.identity;
    private Quaternion footRelativeToRoot = Quaternion.identity;

    public UnityEvent onFootstep = new UnityEvent(); 

    public IKProceduralFootstep(Quaternion rootRotation, Vector3 footPosition, Quaternion footRotation, Vector3 characterSpaceOffset)
    {
        this.characterSpaceOffset = characterSpaceOffset;
        Reset(rootRotation, footPosition, footRotation);
        footRelativeToRoot = Quaternion.Inverse(rootRotation) * rotation;
    }

    public IKProceduralFootstep()
    {
        
    }

    public void Reset(Quaternion rootRotation, Vector3 footPosition, Quaternion footRotation)
    {
        position = footPosition;
        rotation = footRotation;
        stepFrom = position;
        stepTo = position;
        stepFromRot = rotation;
        stepToRot = rotation;
        stepToRootRot = rootRotation;
        stepProgress = 1f;
    }


    //以From 到To按比例走StateProgress做Lerp

    public void UpdateStepping( Quaternion calcModuleForwardRotation, float stateProgess)
    {
        stepProgress = stateProgess;

    }

    public void UpdateStanding(Quaternion calcModuleForwardRotation, float setupRelaxLegTwistMinAngle, float stateProgess)
    {
        
    }

    public void StepTo(Vector3 switchstepTo, Quaternion calcModuleForwardRotation, float setupStepThreshold)
    {
        
        stepFrom = position;
        stepTo = switchstepTo;
        stepFromRot = rotation;
        stepToRootRot = calcModuleForwardRotation;
        stepToRot = calcModuleForwardRotation * footRelativeToRoot;
        stepProgress = 0f;
        
    }
//不要了
    public void Update(InterpolationMode stepInterpolation, UnityEvent onStep)
    {
        if (!isStepping) return;

        if (stepProgress >= 1f) onStep.Invoke ();

        float stepProgressSmooth = RootMotion.Interp.Float(stepProgress, stepInterpolation);

        position = Vector3.Lerp(stepFrom, stepTo, stepProgressSmooth);
        rotation = Quaternion.Lerp(stepFromRot, stepToRot, stepProgressSmooth);
    }

    public void SimulateUpdate(InterpolationMode stepInterpolation, UnityEvent onStep, float deltaTime)
    {
  
        
        if (stepProgress >= 1f) onStep.Invoke ();
        
        stepProgress += stepSpeed * deltaTime;
        float stepProgressSmooth = RootMotion.Interp.Float(stepProgress, stepInterpolation);

        position = Vector3.Lerp(stepFrom, stepTo, stepProgressSmooth);
        rotation = Quaternion.Lerp(stepFromRot, stepToRot, stepProgressSmooth);
    }
}