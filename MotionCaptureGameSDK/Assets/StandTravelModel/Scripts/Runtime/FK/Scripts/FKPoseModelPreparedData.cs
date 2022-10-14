using System.Collections;
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
            if(Application.isPlaying)
            {
                var layer = 1;
                var animator = transform.GetComponent<Animator>();
                var weight = animator.GetLayerWeight(layer);
                animator.SetLayerWeight(layer, 0);

                StartCoroutine(BakeDataPost(animator, layer, weight));
            }
        }

        private IEnumerator BakeDataPost(Animator animator, int layer, float weight)
        {
            yield return new WaitForSeconds(0.5f);
            var enumsAll = System.Enum.GetValues(typeof(EFKType));
            var eFKTypes = new EFKType[enumsAll.Length];
            enumsAll.CopyTo(eFKTypes, 0);
            rotationCorrects = FKPoseModelRotateCorrectsGetter.GetCorrects(animator, eFKTypes);
            animator.SetLayerWeight(layer, weight);
        }
    }
}