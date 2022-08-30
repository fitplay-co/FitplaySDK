using System.Collections.Generic;
using StandTravelModel.Scripts.Runtime.Core;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.MotionModel
{
    public interface IMotionModel
    {
        void OnFixedUpdate();
        void OnUpdate(List<Vector3> keyPoints);
        void OnLateUpdate();
        void Clear();
        AnchorController GetAnchorController();
        List<Vector3> GetKeyPoints();
        Animator GetAnimator();
    }
}