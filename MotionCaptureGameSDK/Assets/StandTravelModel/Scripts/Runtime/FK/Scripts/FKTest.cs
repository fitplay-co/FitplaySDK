using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Scripts
{
    [ExecuteInEditMode]
    public class FKTest : MonoBehaviour
    {
        [SerializeField] private Animator lord;
        [SerializeField] private Animator slave;
        [SerializeField] private Quaternion[] bias;
        [SerializeField] private Transform[] boneTrans;

        [SerializeField] private bool inited;
        [SerializeField] private HumanBodyBones[] bones = {
            HumanBodyBones.LeftShoulder,
            HumanBodyBones.LeftUpperArm,
            HumanBodyBones.LeftLowerArm,
            HumanBodyBones.LeftHand
        };

        private void Update() {
            if(lord != null && slave != null)
            {
                if(inited)
                {
                    for(int i = 0; i < bones.Length; i++)
                    {
                        var bone = bones[i];
                        var target = lord.GetBoneTransform(bone);
                        var follow = slave.GetBoneTransform(bone);
                        follow.rotation = target.rotation * bias[i];
                    }
                }
            }
        }

        public void StartInit()
        {
            if(!inited)
            {
                inited = true;
                bias = new Quaternion[bones.Length];
                boneTrans = new Transform[bones.Length];

                for(int i = 0; i < bones.Length; i++)
                {
                    Init(bones[i], i);
                    InitBoneTrans(bones[i], i);
                }
            }
        }

        private void Init(HumanBodyBones bodyBones, int index)
        {
            var boneTarget = lord.GetBoneTransform(bodyBones);
            var boneFollow = slave.GetBoneTransform(bodyBones);
            Init(boneTarget, boneFollow, index);
        }

        private void Init(Transform target, Transform follow, int index)
        {
            bias[index] = Quaternion.Inverse(target.rotation) * follow.rotation;
        }

        private void InitBoneTrans(HumanBodyBones bodyBones, int index)
        {
            boneTrans[index] = lord.GetBoneTransform(bodyBones);
        }
    }
}