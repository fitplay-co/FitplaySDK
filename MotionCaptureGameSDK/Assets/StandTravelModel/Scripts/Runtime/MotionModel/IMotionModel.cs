using System.Collections.Generic;
using UnityEngine;

namespace StandTravelModel.Core
{
    public interface IMotionModel
    {
        void OnUpdate(List<Vector3> keyPoints);
        void OnLateUpdate();
        void Clear();
        AnchorController GetAnchorController();
    }
}