using System;
using System.Collections.Generic;
using UnityEngine;

public interface IActionReconComp
{
    void OnUpdate(List<Vector3> keyPoints);
    void SetAction(Action<bool> onAction);
    void SetDebug(bool isDebug);
}