namespace SEngineCharacterController
{
    public class WalkState :  State<FsmComponent>
    {
        public WalkState(FsmComponent owner) : base(owner)
        {
            
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
        }
    }
}