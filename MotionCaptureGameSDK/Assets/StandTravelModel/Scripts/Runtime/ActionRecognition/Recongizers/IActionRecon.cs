using System.Collections.Generic;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.Recongizers
{
    public interface IActionRecon
    {
        void OnUpdate(List<Vector3> keyPoints);
        void SetDebug(bool isDebug);
    }
}