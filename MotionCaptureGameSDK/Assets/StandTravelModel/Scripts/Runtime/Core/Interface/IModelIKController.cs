using System.Collections.Generic;
using UnityEngine;

namespace StandTravelModel.Core.Interface
{
    public interface IModelIKController
    {
        public void InitializeIKTargets(Transform ikPointsRoot);
        public void ClearFakeNodes();
        public void UpdateIKTargetsData(List<Vector3> keyPointsList);
        public void ChangeLowerBodyIKWeight(float weight);
        public void ChangeUpperBodyIKWeight(float weight);
    }
}