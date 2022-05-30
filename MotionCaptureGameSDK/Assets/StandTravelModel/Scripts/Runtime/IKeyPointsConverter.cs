using System.Collections.Generic;
using UnityEngine;

public interface IKeyPointsConverter
{
    void ConvertKeyPoints(List<Vector3> keyPoints);
}
