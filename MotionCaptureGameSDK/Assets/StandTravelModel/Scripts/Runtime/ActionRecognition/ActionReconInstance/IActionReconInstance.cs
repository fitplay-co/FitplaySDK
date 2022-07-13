using System.Collections.Generic;
using UnityEngine;

public interface IActionReconInstance
{
    void OnUpdate(List<Vector3> keyPoints);
    ActionId GetActionId();
    void SetDebug(bool isDebug);
}