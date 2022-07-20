using UnityEngine;

namespace SEngineCharacterController
{
    public class ModelComponent : Component
    {
        public GameObject Model { get; private set; }

        public Transform ModelTransform => Model.transform;
        
        public CharacterController CharacterController{ get; private set; }

        public Animator Animator { get; private set; }

        public Transform ForwardCube { get; private set; }

        public override void OnInit(object initData)
        {
            Model = (GameObject)initData;
            CharacterController = Model.GetComponent<CharacterController>();
            Animator = Model.GetComponent<Animator>();
            ForwardCube = ModelTransform.Find("Character1_Reference/Character1_Hips/Character1_Spine/Character1_Spine1/Character1_Spine2/Character1_Neck/ForwardCube");
            base.OnInit(initData);
        }
    }
}