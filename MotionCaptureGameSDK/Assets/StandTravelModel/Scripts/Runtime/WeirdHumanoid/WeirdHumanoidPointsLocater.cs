using UnityEngine;
using MotionCaptureBasic.Interface;

namespace WeirdHumanoid
{
    public class WeirdHumanoidPointsLocater : MonoBehaviour
    {
        [SerializeField] private Transform nose;
        [SerializeField] private Transform leftShoulder;
        [SerializeField] private Transform rightShoulder;
        [SerializeField] private Transform leftElbow;
        [SerializeField] private Transform rightElbow;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;
        [SerializeField] private Transform leftIndex;
        [SerializeField] private Transform rightIndex;
        [SerializeField] private Transform leftHip;
        [SerializeField] private Transform rightHip;
        [SerializeField] private Transform leftKnee;
        [SerializeField] private Transform rightKnee;
        [SerializeField] private Transform leftFoot;
        [SerializeField] private Transform rightFoot;
        [SerializeField] private Transform leftFootIndex;
        [SerializeField] private Transform rightFootIndex;

        private float[] limbOffsets;
        private Transform[] points;

        private void Awake() {
            TryInitPoints();
            TryInitOffsets();
        }

        public float GetLimbOffset(GameKeyPointsType keyPointsType)
        {
            if(limbOffsets != null)
            {
                return limbOffsets[(int)keyPointsType];
            }
            return 0;
        }

        private Transform GetPoint(GameKeyPointsType keyPointsType)
        {
            return points[(int)keyPointsType];
        }

        private void TryInitPoints()
        {
            if(points == null)
            {
                var pointsTypes = System.Enum.GetNames(typeof(GameKeyPointsType));
                points = new Transform[pointsTypes.Length];

                points[(int)GameKeyPointsType.Nose] = nose;
                points[(int)GameKeyPointsType.LeftShoulder] = leftShoulder;
                points[(int)GameKeyPointsType.RightShoulder] = rightShoulder;
                points[(int)GameKeyPointsType.LeftElbow] = leftElbow;
                points[(int)GameKeyPointsType.RightElbow] = rightElbow;
                points[(int)GameKeyPointsType.LeftHip] = leftHip;
                points[(int)GameKeyPointsType.RightHip] = rightHip;
                points[(int)GameKeyPointsType.LeftFoot] = leftFoot;
                points[(int)GameKeyPointsType.RightFoot] = rightFoot;
                points[(int)GameKeyPointsType.LeftKnee] = leftKnee;
                points[(int)GameKeyPointsType.RightKnee] = rightKnee;
                points[(int)GameKeyPointsType.LeftFootIndex] = leftFootIndex;
                points[(int)GameKeyPointsType.RightFootIndex] = rightFootIndex;
                points[(int)GameKeyPointsType.LeftHand] = leftHand;
                points[(int)GameKeyPointsType.RightHand] = rightHand;
                points[(int)GameKeyPointsType.LeftIndex] = leftIndex;
                points[(int)GameKeyPointsType.RightIndex] = rightIndex;
            }
        }

        private void TryInitOffsets()
        {
            if(limbOffsets == null)
            {
                var pointsTypes = System.Enum.GetValues(typeof(GameKeyPointsType));
                limbOffsets = new float[pointsTypes.Length];
                for(int i = 0; i < limbOffsets.Length; i++)
                {
                    var pointType = (GameKeyPointsType)i;
                    var anchorType = WeirdHumanoidLimbRelativeGetter.GetAnchorKeyPointType(pointType);
                    var point = GetPoint(pointType);
                    var anchorPos = pointType != anchorType ? GetPointPosition(anchorType) : GetChestPosition();
                    if(point != null)
                    {
                        limbOffsets[i] = Vector3.Distance(point.position, anchorPos);
                        Debug.Log(pointType + "->" + anchorType + "|" + limbOffsets[i]);
                    }
                }
            }
        }

        private Vector3 GetPointPosition(GameKeyPointsType keyPointsType)
        {
            return points[(int)keyPointsType].position;
        }

        private Vector3 GetChestPosition()
        {
            var leftShoulder = GetPoint(GameKeyPointsType.LeftShoulder);
            var rightShoulder = GetPoint(GameKeyPointsType.RightShoulder);
            var leftHip = GetPoint(GameKeyPointsType.LeftHip);
            var rightHip = GetPoint(GameKeyPointsType.RightHip);
            return (leftShoulder.position + rightShoulder.position + leftHip.position + rightHip.position) * 0.25f;
        }
    }
}