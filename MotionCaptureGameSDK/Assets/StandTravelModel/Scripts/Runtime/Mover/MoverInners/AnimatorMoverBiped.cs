using UnityEngine;

public class AnimatorMoverBiped : IAnimatorMoverBiped
{
    private const float footHeightDetach = 0.02f;

    protected const int footIndexLeft = -1;
    protected const int footIndexRight = 1;

    private int curFootIndex;
    private Vector3 anchorPos;
    private Transform footLeft;
    private Transform footRight;
    private Transform transform;
    private IAnimatorMoverReactor moverReactor;

    public AnimatorMoverBiped(Transform transform)
    {
        var animator = transform.GetComponent<Animator>();
        this.footLeft = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        this.footRight = animator.GetBoneTransform(HumanBodyBones.RightFoot);
        this.transform = transform;
        this.moverReactor = transform.GetComponent<IAnimatorMoverReactor>();
    }

    public void OnUpdate(AnimatorStateInfo stateInfo)
    {
        UpdateTouchingFoot(stateInfo);
        UpdatePosWithAnchor();
    }

    protected virtual int GetTouchingFoot(AnimatorStateInfo stateInfo)
    {
        var heightGap = footLeft.position.y - footRight.position.y;
        if(heightGap < -footHeightDetach)
        {
            return footIndexLeft;
        }

        if(heightGap > footHeightDetach)
        {
            return footIndexRight;
        }

        return 0;
    }

    private void UpdatePosWithAnchor()
    {
        if(curFootIndex != 0)
        {
            var deltaPos = GetFootPos(curFootIndex) - anchorPos;
            deltaPos.y = 0;

            moverReactor.SetAnimatorDelta(-deltaPos);
        }
    }

    private void UpdateTouchingFoot(AnimatorStateInfo stateInfo)
    {
        var touchingFoot = GetTouchingFoot(stateInfo);
        if(touchingFoot != curFootIndex)
        {
            curFootIndex = touchingFoot;
            anchorPos = GetAnchorPos(curFootIndex);
        }
    }

    private Transform GetAnchor(int foot)
    {
        if(foot == footIndexRight)
        {
            return footRight;
        }

        if(foot == footIndexLeft)
        {
            return footLeft;
        }

        return transform;
    }

    private Vector3 GetAnchorPos(int foot)
    {
        return GetAnchor(foot).position;
    }

    private Vector3 GetFootPos(int foot)
    {
        return GetAnchor(foot).position;
    }

    public void OnStart()
    {
        moverReactor.OnAnimatorMoveStart();
    }
}