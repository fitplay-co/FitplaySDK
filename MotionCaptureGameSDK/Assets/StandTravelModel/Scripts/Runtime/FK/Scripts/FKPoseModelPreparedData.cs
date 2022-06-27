using UnityEngine;
using MotionCaptureBasic.OSConnector;

namespace FK
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