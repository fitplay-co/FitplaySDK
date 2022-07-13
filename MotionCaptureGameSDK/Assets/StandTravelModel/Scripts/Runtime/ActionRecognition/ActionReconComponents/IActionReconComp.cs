using System;
using System.Collections.Generic;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconComponents
{
    public interface IActionReconComp
    {
        void OnUpdate(List<Vector3> keyPoints);
        void SetAction(Action<bool> onAction);
        void SetDebug(bool isDebug);
    }
}