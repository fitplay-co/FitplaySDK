using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorMoverUpdater : MonoBehaviour, IAnimatorMoverReactor
{
    private Vector3? moveDest;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        animator.applyRootMotion = true;
    }

    /// <summary>
    /// Callback for processing animation movements for modifying root motion.
    /// </summary>
    void OnAnimatorMove()
    {
        
    }

    private void FixedUpdate() {
        if(moveDest.HasValue)
        {
            transform.position = moveDest.Value;
            moveDest = null;
        }
    }

    public void SetMoveDest(Vector3 moveDest)
    {
        this.moveDest = moveDest;
    }
}