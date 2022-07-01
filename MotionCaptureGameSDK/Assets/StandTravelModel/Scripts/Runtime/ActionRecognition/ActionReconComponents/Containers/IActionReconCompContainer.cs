using System.Collections.Generic;
using UnityEngine;

public interface IActionReconCompContainer
{
    void AddReconComp(IActionReconComp comp);
    void UpdateReconComps(List<Vector3> keyPoints);
    void SetDebug(bool isDebug);
}