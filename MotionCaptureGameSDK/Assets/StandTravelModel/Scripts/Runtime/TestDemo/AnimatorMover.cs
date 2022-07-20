using System;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.TestDemo
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorMover : MonoBehaviour
    {
        private const int footIndexLeft = -1;
        private const int footIndexRight = 1;
        private const float footHeightDetach = 0.02f;
        private CharacterController characterController;
        private StandTravelModelManager standTravelModelManager;
        private Vector3 _velocity;
        public Vector3 velocity => _velocity;

        private const float groundedOffset = 0.1f;

        private bool _isGrounded = true;
        public bool isGrounded => _isGrounded;
        
        private float _verticalVelocity;
        private float _terminalVelocity = -53.0f;

        [SerializeField] private bool isUseCharacterController;
        [SerializeField] private int curFootIndex;
        [SerializeField] private Vector3 footPos;
        [SerializeField] private Vector3 deltaPos;
        [SerializeField] private Vector3 anchorPos;
        [SerializeField] private Transform footLeft;
        [SerializeField] private Transform footRight;
        [SerializeField] private float speedMultiplier = 1;
        [SerializeField] private float gravity = -10;

        private void Awake() {
            var animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            standTravelModelManager = GetComponent<StandTravelModelManager>();
            footLeft = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            footRight = animator.GetBoneTransform(HumanBodyBones.RightFoot);
        }

        private void FixedUpdate() {
            if (standTravelModelManager.currentMode == MotionMode.Stand)
            {
                _velocity = Vector3.zero;
                _verticalVelocity = 0;
            }
            else
            {
                var fixDeltaTime = Time.fixedDeltaTime;
                UpdateTouchingFoot();
                UpdatePosWithAnchor(fixDeltaTime);
                UpdateVerticalMove(fixDeltaTime);
            }
        }

        private void Update()
        {
            if (!isUseCharacterController)
            {
                return;
            }
            var dt = Time.deltaTime;
            var deltaMovement = _velocity * (speedMultiplier * dt) + new Vector3(0, _verticalVelocity * dt, 0);
            characterController.Move(deltaMovement);
        }

        /// <summary>
        /// Callback for processing animation movements for modifying root motion.
        /// </summary>
        void OnAnimatorMove()
        {
            //override the root motion
        }

        private void UpdateVerticalMove(float dt)
        {
            if (transform.position.y - standTravelModelManager.groundHeight > groundedOffset)
            {
                _isGrounded = false;
                if (_verticalVelocity > _terminalVelocity)
                {
                    _verticalVelocity += gravity * dt;
                }
            }
            else
            {
                _isGrounded = true;
                _verticalVelocity = -1;
            }
        }

        private void UpdatePosWithAnchor(float dt)
        {
            if (curFootIndex != 0)
            {
                footPos = GetFootPos(curFootIndex);
                deltaPos = footPos - anchorPos;
                _velocity = -deltaPos / dt;
                _velocity.y = 0;
                if (!isUseCharacterController)
                {
                    transform.position -= deltaPos;
                }
            }
            else
            {
                _velocity = Vector3.zero;
            }
        }

        private void UpdateTouchingFoot()
        {
            var touchingFoot = GetTouchingFoot();
            if(touchingFoot != curFootIndex)
            {
                curFootIndex = touchingFoot;
                anchorPos = GetAnchorPos(curFootIndex);
            }
        }

        private int GetTouchingFoot()
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
    }
}