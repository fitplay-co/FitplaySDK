using System.Collections.Generic;
using StandTravelModel.Core;
using UnityEngine;

namespace StandTravelModel.MotionModel
{
    public interface IMotionModel
    {
        void OnFixedUpdate();
        void OnUpdate(List<Vector3> keyPoints);
        void OnLateUpdate();
        void Clear();
        AnchorController GetAnchorController();
    }
}