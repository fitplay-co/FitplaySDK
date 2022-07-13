using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class FKPoseModelPreparedData : MonoBehaviour
    {
        [SerializeField] private Quaternion[] rotationCorrects;

        public Quaternion[] GetRotationCorrects()
        {
            return rotationCorrects;
        }

        public void BakeData()
        {
            var animator = transform.GetComponent<Animator>();
            var enumsAll = System.Enum.GetValues(typeof(EFKType));
            var eFKTypes = new EFKType[enumsAll.Length];
            enumsAll.CopyTo(eFKTypes, 0);
            rotationCorrects = FKPoseModelRotateCorrectsGetter.GetCorrects(animator, eFKTypes);
        }
    }
}