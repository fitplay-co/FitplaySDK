using System.Collections.Generic;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime
{
    public interface IKeyPointsConverter
    {
        void ConvertKeyPoints(List<Vector3> keyPoints);
    }
}
