using System.Collections.Generic;
using UnityEngine;

namespace StandTravelModel.Core.Interface
{
    public interface IModelIKController
    {
        public void InitializeIKTargets(Transform ikPointsRoot);
        public void UpdateIKTargetsData(List<Vector3> keyPointsList);
        public void ChangeLowerBodyIKWeight(float weight);
    }
}