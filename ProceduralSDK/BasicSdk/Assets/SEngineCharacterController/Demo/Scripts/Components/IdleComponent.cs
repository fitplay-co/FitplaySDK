namespace SEngineCharacterController
{
    public class IdleComponent : Component
    {
        public override void OnInit()
        {
            base.OnInit();
            Launcher.Instance.RegisterTick(OnTick);
        }

        private void OnTick(float dt)
        {
            
        }
    }
}