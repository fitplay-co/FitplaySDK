using SEngineBasic;
using UnityEngine;

namespace SEngineCharacterController
{
    public class JointPoint
    {
        // Bones
        public Transform Transform;
    }
    public class TransformComponent : Component
    {
        private ModelComponent modelComponent;
        private InputComponent inputComponent;
        public bool IsGrounded { get; private set; }
        
        private LayerMask groundLayers = default;
        
        public Vector3 Position
        {
            get => modelComponent.ModelTransform.position;
            set => modelComponent.ModelTransform.position = value;
        }

        public Vector3 RotationVector3
        {
            get => modelComponent.ModelTransform.rotation.eulerAngles;
        }

        public Quaternion RotationQuaternion
        {
            get => modelComponent.ModelTransform.rotation;
            set => modelComponent.ModelTransform.rotation = value;
        }

        public Vector3 Scale
        {
            get => modelComponent.ModelTransform.localScale;
            set => modelComponent.ModelTransform.localScale = value;
        }
        public override void OnInit()
        {
            modelComponent = Owner.GetComponent<ModelComponent>();
            inputComponent = Owner.GetComponent<InputComponent>();
            InitJointPoint();
            Launcher.Instance.RegisterFixedTick(OnFixedTick);
            base.OnInit();
        }

        private void OnFixedTick(float dt)
        {
            PoseUpdate();
        //    JumpAndGravity(dt);
         //   GroundedCheck();
            Turn(dt);
            Move(dt);
        }

        private int readyTime = 2;
        private Quaternion initNeckRotation;
        private Quaternion initNeckRotationDiff;

        private void PoseUpdate()
        {
            var rotations = inputComponent.Rotations;
            if(rotations == null || rotations.Count < EFKType.Count.Int() - 1) return;

            if (readyTime > 0)
            {
                readyTime--;
                if (readyTime == 1)
                {
                    initNeckRotation = jointPoints[EFKType.Neck.Int()].Transform.rotation;
                }

                for (var i = 0; i < EFKType.Count.Int() - 1; i++)
                {
                    if(i == EFKType.RFoot.Int() || i == EFKType.LFoot.Int() || i == EFKType.Neck.Int()) continue;
                    jointPoints[i].Transform.rotation = RotationQuaternion * rotations[i].Rotation();
                }
                
                if (readyTime == 0)
                {
                    var neckTempRotation = jointPoints[EFKType.Neck.Int()].Transform.rotation;
                    initNeckRotationDiff = Quaternion.Euler(0,initNeckRotation.eulerAngles.y - neckTempRotation.eulerAngles.y, 0);
                }
            }
            else
            {
                for (var i = 0; i < EFKType.Count.Int() - 1; i++)
                {
                    if(i == EFKType.RFoot.Int() || i == EFKType.LFoot.Int() || i == EFKType.Neck.Int()) continue;
                    jointPoints[i].Transform.rotation = RotationQuaternion * initNeckRotationDiff * rotations[i].Rotation();
                }
            }
        }
        private float verticalVelocity;
        private readonly float terminalVelocity = 53.0f;
        private readonly float jumpTimeout = 0.3f;
        private readonly float fallTimeout = 0.15f;
        private float jumpTimeoutDelta;
        private float fallTimeoutDelta;
        private readonly float gravity = Physics.gravity.y;
        private readonly float jumpHeight = 1.2f;
        private void JumpAndGravity(float dt)
        {
            if (IsGrounded)
            {
                // stop our velocity dropping infinitely when grounded
                if (verticalVelocity < 0.0f)
                {
                    verticalVelocity = -2f;
                }
                //jump
                if (inputComponent.IsJump && jumpTimeoutDelta <= 0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
                }
            }
            else
            {
                // reset the jump timeout timer
                jumpTimeoutDelta = jumpTimeout;
                
                // fall timeout
                if (fallTimeoutDelta >= 0.0f)
                {
                    fallTimeoutDelta -= dt;
                }
            }
            
            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity += gravity * dt;
            }
        }

        private void GroundedCheck()
        {
            var groundedRadius = modelComponent.CharacterController.radius;
            var groundedOffset = groundedRadius * -0.5f;
            var spherePosition = new Vector3(Position.x, Position.y - groundedOffset, Position.z);
            IsGrounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
        }

        private float rotationVelocity;
        private void Turn(float dt)
        {
            if (inputComponent.LJoystick != Vector2.zero)
            {
                var inputDirection = new Vector3(inputComponent.LJoystick.x, 0.0f, inputComponent.LJoystick.y).normalized;
                //var targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
                //var rotation = Mathf.SmoothDampAngle(RotationVector3.y, targetRotation, ref rotationVelocity,1f);
                //RotationQuaternion = Quaternion.Euler(0, rotation, 0);
                var quaDir = Quaternion.LookRotation(inputDirection);
                
                Debug.LogError($"{inputComponent.LJoystick}  {inputDirection}");
                RotationQuaternion = Quaternion.Lerp(RotationQuaternion,quaDir, dt);
            }
        }

        private void Move(float dt)
        {
            var direction = modelComponent.ForwardCube.forward;
            var forwardMotion = direction.normalized * (inputComponent.Speed * dt);
            //Debug.LogError($"forwardMotion:{direction},{forwardMotion}");
            var verticalMotion = Vector3.zero;//new Vector3(0.0f, verticalVelocity, 0.0f) * dt;
            modelComponent.CharacterController.Move(forwardMotion);
        }

        private JointPoint[] jointPoints;
        private void InitJointPoint()
        {
            var animator = modelComponent.Animator;
            animator.enabled = false;
            jointPoints = new JointPoint[EFKType.Count.Int() ];
            for (var i = 0; i < EFKType.Count.Int(); i++) jointPoints[i] = new JointPoint();

            // Right Arm
            jointPoints[EFKType.RShoulder.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
            jointPoints[EFKType.RUpArm.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
            jointPoints[EFKType.RLowArm.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
            jointPoints[EFKType.RHand.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.RightHand);

            // Left Arm
            jointPoints[EFKType.LShoulder.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
            jointPoints[EFKType.LUpArm.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            jointPoints[EFKType.LLowArm.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            jointPoints[EFKType.LHand.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.LeftHand);

            // etc
            jointPoints[EFKType.Neck.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.Neck);
            jointPoints[EFKType.Spine.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.Spine);
           //jointPoints[EFKType.Root.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.);

            // Right Leg
            jointPoints[EFKType.RHip.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.Hips);
            jointPoints[EFKType.RUpLeg.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
            jointPoints[EFKType.RLowLeg.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
            jointPoints[EFKType.RFoot.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.RightFoot);

            // Left Leg
            jointPoints[EFKType.LHip.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.Hips);
            jointPoints[EFKType.LUpLeg.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
            jointPoints[EFKType.LLowLeg.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
            jointPoints[EFKType.LFoot.Int()].Transform = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        }
    }
}