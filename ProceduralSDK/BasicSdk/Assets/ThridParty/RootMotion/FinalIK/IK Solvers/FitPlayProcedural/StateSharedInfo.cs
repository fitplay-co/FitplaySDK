using UnityEngine;

namespace RootMotion.FinalIK.FitPlayProcedural
{
    public class StateSharedInfo
    {
        public CurrentFoot LastStepedFoot;
        public IKProceduralFootstep[] footsteps = new IKProceduralFootstep[2];


        public void SetLeftFootStep(Quaternion rotation, Vector3 position, Quaternion quaternion, Vector3 footOffset)
        {
            footsteps[0] = new IKProceduralFootstep(rotation, position, quaternion, footOffset);
        }
        public void SetRightFootStep(Quaternion rotation, Vector3 position, Quaternion quaternion, Vector3 footOffset)
        {
            footsteps[1] = new IKProceduralFootstep(rotation, position, quaternion, footOffset);
        }
    }
}