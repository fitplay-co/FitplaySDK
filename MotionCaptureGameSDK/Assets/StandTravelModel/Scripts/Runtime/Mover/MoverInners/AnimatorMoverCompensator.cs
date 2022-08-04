using UnityEngine;

[System.Serializable]
public class AnimatorMoverCompensator
{
    [SerializeField] private AnimationCurve speedCurve;

    public float GetCompensation(float stepProgress)
    {
        /* var isRangeNotFlip = min < max;
        var progressInRange = stepProgress >= min && stepProgress <= max;
        if(
            (isRangeNotFlip && (stepProgress >= min && stepProgress <= max)) ||
            (!isRangeNotFlip && (stepProgress >= min || stepProgress <= max))
        )
        {
            return speedBase;
        }
        return 0; */
        return speedCurve.Evaluate(stepProgress);
    }

    /* private float GetCompensationProgress(float stepProgress)
    {
        if(min < max)
        {
            return (stepProgress - min) / (max - min);
        }
        else
        {
            return (stepProgress - max) / (max + (1 - min));
        }
    } */
}