using System.Collections.Generic;
using UnityEngine;

public interface IActionRecon
{
    void OnUpdate(List<Vector3> keyPoints);
}