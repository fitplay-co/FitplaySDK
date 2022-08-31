using StandTravelModel.Scripts.Runtime.Mover;
using StandTravelModel.Scripts.Runtime.Mover.MoverInners;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.TestDemo
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorMover : MonoBehaviour, IAnimatorMoverReactor
    {
        private const int footIndexLeft = -1;
        private const int footIndexRight = 1;
        private const float footHeightDetach = 0.02f;
        private StandTravelModelManager standTravelModelManager;
        private Vector3 _velocity;

        public Vector3 velocity => (_velocity + new Vector3(0, _verticalVelocity, 0));
        private AnimatorMoverUpdater moverUpdater;

        private const float groundedOffset = 0.1f;

        private bool _isGrounded = true;
        public bool isGrounded => _isGrounded;
        
        private float _verticalVelocity;
        private float _terminalVelocity = -53.0f;

        [Tooltip("是否利用AnimatorMover来移动。如果不勾则不通过该脚本控制移动")]
        [SerializeField] private bool isSelfMoveControl;

        [Tooltip("指定需要控制的characterController")] [SerializeField]
        private CharacterController characterController;
        
        [SerializeField] private int curFootIndex;
        [SerializeField] private float gravity = -10;
        [SerializeField] private Vector3 anchorPos;

        private Transform footLeft;
        private Transform footRight;

        private void Awake() {
            var animator = GetComponent<Animator>();
            animator.applyRootMotion = true;
            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }
            
            standTravelModelManager = GetComponent<StandTravelModelManager>();
            footLeft = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            footRight = animator.GetBoneTransform(HumanBodyBones.RightFoot);
            
            moverUpdater = new AnimatorMoverUpdater(characterController);
            moverUpdater.SetParameter(isSelfMoveControl);
        }

        private void FixedUpdate() {
            if (standTravelModelManager.currentMode == MotionMode.Travel)
            {
                var fixDeltaTime = Time.fixedDeltaTime;
                UpdateVerticalMove(fixDeltaTime);
            }
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            if (standTravelModelManager.currentMode == MotionMode.Stand)
            {
                _velocity = Vector3.zero;
                _verticalVelocity = 0;
            }
            else
            {
                moverUpdater.OnUpdate();
                _velocity = moverUpdater.GetMoveSpeed(deltaTime);
            }
            if (isSelfMoveControl)
            {
                var deltaMovement = new Vector3(0, _verticalVelocity * deltaTime, 0);
                characterController.Move(deltaMovement);
            }
        }

        /// <summary>
        /// Callback for processing animation movements for modifying root motion.
        /// </summary>
        void OnAnimatorMove()
        {
            //override the root motion
        }

        public float GetRunSpeedScale()
        {
            return standTravelModelManager.GetRunSpeedScale();
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

        public void SetAnimatorDelta(Vector3 moveDelta)
        {
            moverUpdater.SetMoveDelta(moveDelta);
        }

        public void OnAnimatorMoveStart()
        {

        }
    }
}
