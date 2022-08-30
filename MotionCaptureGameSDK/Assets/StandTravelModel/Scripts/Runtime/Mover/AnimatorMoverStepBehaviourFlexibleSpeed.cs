using StandTravelModel.Scripts.Runtime;
using StandTravelModel.Scripts.Runtime.Mover.MoverInners;
using UnityEngine;

public class AnimatorMoverStepBehaviourFlexibleSpeed : AnimatorMoverStepBehaviourFixedSpeed
{
    [SerializeField] private bool speedScaleFromPanel;
    [SerializeField] private float speedScale;
    [SerializeField] private float progressLeftStart;
    [SerializeField] private float progressLeftEnd;
    [SerializeField] private AnimatorMoverCompensator[] compensators;

    private IAnimatorMoverBiped curMover;
    private AnimatorMoverOSSpeed moverOSSpeed;
    private StandTravelModelManager standTravelModelManager;
    private AnimatorMoverBipedStepProgress moverBipedStepProgress;

    protected override IAnimatorMoverBiped CreateAnimatorMover(Animator animator)
    {
        moverOSSpeed = new AnimatorMoverOSSpeed(GetSpeed, animator.transform);
        moverBipedStepProgress = new AnimatorMoverBipedStepProgress(animator.transform, progressLeftStart, progressLeftEnd, compensators, speedScale, speedScaleFromPanel);
        standTravelModelManager = animator.GetComponent<StandTravelModelManager>();
        return TrySwitchMover(standTravelModelManager.GetUseOSSpeed());
    }

    private IAnimatorMoverBiped TrySwitchMover(bool useOSSpeed)
    {
        if(ChooseMover(useOSSpeed) != curMover)
        {
            curMover = ChooseMover(useOSSpeed);
        }
        return curMover;
    }

    private IAnimatorMoverBiped ChooseMover(bool useOSSpeed)
    {
        if(useOSSpeed)
        {
            return moverOSSpeed;
        }
        return moverBipedStepProgress;
    }
}
