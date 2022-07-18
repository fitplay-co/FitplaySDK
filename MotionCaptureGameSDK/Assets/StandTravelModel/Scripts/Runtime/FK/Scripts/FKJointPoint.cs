using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Scripts
{
    [System.Serializable]
    public class FKJointPoint
    {
        // Bones
        public Transform Transform;

        public Quaternion InitRotation;
        public Quaternion Inverse;
        public Quaternion InverseRotation;
        
        public FKJointPoint Child;
        public FKJointPoint Parent;
    }
}