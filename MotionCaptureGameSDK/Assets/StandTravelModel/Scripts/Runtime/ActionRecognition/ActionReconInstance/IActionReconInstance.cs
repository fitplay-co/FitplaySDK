using System.Collections.Generic;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconInstance
{
    public interface IActionReconInstance
    {
        void OnUpdate(List<Vector3> keyPoints);
        ActionId GetActionId();
        void SetDebug(bool isDebug);
    }
}