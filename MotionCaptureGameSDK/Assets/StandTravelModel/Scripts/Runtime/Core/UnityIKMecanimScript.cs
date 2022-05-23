using UnityEngine;
using UnityEngine.Serialization;

namespace MotionCapture.StandTravelModel.Editor
{
    public class UnityIKMecanimScript: MonoBehaviour
    {
        public Animator myAnimator;
    
        // variables for te IK joint points
        public Transform IK_rightHandTarget;
        public Transform IK_leftHandTarget;
        
        // variables for ashi IK joint points
        public Transform IK_rightFootTarget;
        public Transform IK_leftFootTarget;
        
        // variables for hiji IK joint points
        public Transform IK_rightElbowTarget;
        public Transform IK_leftElbowTarget;

        // variables for hiza IK joint points
        public Transform IK_rightKneeTarget;
        public Transform IK_leftKneeTarget;
    
        public Transform lookPoint;
        
        public float IK_rightHandPositionWeight = 1;
        public float IK_rightHandRotationWeight = 1;
        public float IK_leftHandPositionWeight = 1;
        public float IK_leftHandRotationWeight = 1;
        
        public float IK_rightFootPositionWeight = 1;
        public float IK_rightFootRotationWeight = 1;
        public float IK_leftFootPositionWeight = 1;
        public float IK_leftFootRotationWeight = 1;
        
        public float IK_rightElbowPositionWeight = 1;
        public float IK_leftElbowPositionWeight = 1;
        
        public float IK_rightKneePositionWeight = 1;
        public float IK_leftKneePositionWeight = 1;

        //used for record step sequence
        public Transform FakeAnkleJointLeft;
        public Transform FakeAnkleJointRight;
    
        private void OnAnimatorIK(int layerIndex)
        {
            if (myAnimator == null)
            {
                return;
            }

            if (IK_rightHandTarget != null)
            {
                myAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, IK_rightHandPositionWeight);
                myAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, IK_rightHandRotationWeight);
                myAnimator.SetIKPosition(AvatarIKGoal.RightHand, IK_rightHandTarget.position);
                myAnimator.SetIKRotation(AvatarIKGoal.RightHand, IK_rightHandTarget.rotation);
            }
    
            if (IK_leftHandTarget != null)
            {
                myAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, IK_leftHandPositionWeight);
                myAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, IK_leftHandRotationWeight);
                myAnimator.SetIKPosition(AvatarIKGoal.LeftHand, IK_leftHandTarget.position);
                myAnimator.SetIKRotation(AvatarIKGoal.LeftHand, IK_leftHandTarget.rotation);
            }
            
            if (IK_rightElbowTarget != null)
            {
                myAnimator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, IK_rightElbowPositionWeight);
                myAnimator.SetIKHintPosition(AvatarIKHint.RightElbow, IK_rightElbowTarget.position);
            }
            
            if (IK_rightElbowTarget != null)
            {
                myAnimator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, IK_leftElbowPositionWeight);
                myAnimator.SetIKHintPosition(AvatarIKHint.LeftElbow, IK_leftElbowTarget.position);
            }

            if (IK_rightFootTarget != null)
            {
                myAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, IK_rightFootPositionWeight);
                myAnimator.SetIKRotationWeight(AvatarIKGoal.RightFoot, IK_rightFootRotationWeight);
                myAnimator.SetIKPosition(AvatarIKGoal.RightFoot, IK_rightFootTarget.position);
                myAnimator.SetIKRotation(AvatarIKGoal.RightFoot, IK_rightFootTarget.rotation);
            }
    
            if (IK_leftFootTarget != null)
            {
                myAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, IK_leftFootPositionWeight);
                myAnimator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, IK_leftFootRotationWeight);
                myAnimator.SetIKPosition(AvatarIKGoal.LeftFoot, IK_leftFootTarget.position);
                myAnimator.SetIKRotation(AvatarIKGoal.LeftFoot, IK_leftFootTarget.rotation);
            }
            
            if (IK_rightElbowTarget != null)
            {
                myAnimator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, IK_rightKneePositionWeight);
                myAnimator.SetIKHintPosition(AvatarIKHint.RightKnee, IK_rightKneeTarget.position);
            }
            
            if (IK_rightElbowTarget != null)
            {
                myAnimator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, IK_leftKneePositionWeight);
                myAnimator.SetIKHintPosition(AvatarIKHint.LeftKnee, IK_leftKneeTarget.position);
            }
    
            //same for a head
            if (lookPoint != null)
            {
                myAnimator.SetLookAtWeight(1);
                myAnimator.SetLookAtPosition(lookPoint.transform.position);
            }
        }
    }
}