using System.Collections.Generic;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconComponents.Containers
{
    public interface IActionReconCompContainer
    {
        void AddReconComp(IActionReconComp comp);
        void UpdateReconComps(List<Vector3> keyPoints);
        void SetDebug(bool isDebug);
    }
}