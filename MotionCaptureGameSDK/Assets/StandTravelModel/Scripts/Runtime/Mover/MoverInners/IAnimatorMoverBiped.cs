using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Mover.MoverInners
{
    public interface IAnimatorMoverBiped
    {
        void OnUpdate(AnimatorStateInfo stateInfo);
        void OnStart();
    }
}