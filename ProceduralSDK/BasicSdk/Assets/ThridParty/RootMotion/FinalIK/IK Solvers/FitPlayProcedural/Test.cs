
using SEngineCharacterController;

namespace RootMotion.FinalIK.FitPlayProcedural
{
    public class Test
    {
        public static void Main(string[] args)
        {
            State<ProceduralState> initState = new State<ProceduralState>(null);

            StateMachine<ProceduralState> machine = new StateMachine<ProceduralState>(initState);

        }
        
    }
}