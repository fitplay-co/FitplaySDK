namespace SEngineCharacterController
{
    public class OutputComponent : Component
    {
        public override void OnInit()
        {
            Launcher.Instance.RegisterTick(OnTick);
            base.OnInit();
        }

        private void OnTick(float dt)
        {
            
        }
        
        
    }
}