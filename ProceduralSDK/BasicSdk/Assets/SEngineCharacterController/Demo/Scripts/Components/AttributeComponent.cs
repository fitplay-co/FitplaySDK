using UnityEngine;

namespace SEngineCharacterController
{
    public class AttributeComponent : Component
    {
        public CharacterType CharacterType { get; private set; }
        public string Name { get; set; }
        public HealthPoint HealthPoint { get; private set; }

        public override void OnInit(object characterType, object name)
        {
            HealthPoint = new HealthPoint();
            HealthPoint.SetBase(1000);
            HealthPoint.SetMaxValue(1000);

            CharacterType = (CharacterType)characterType;
            Name = (string)name;
            Launcher.Instance.RegisterTick(OnTick);
            base.OnInit();
        }

        private void OnTick(float dt)
        {
            //HealthPoint.Minus(1);
            //Debug.LogError($"name:{Name} value:{HealthPoint.Value} percent:{HealthPoint.Percent()}");
        }
    }
}