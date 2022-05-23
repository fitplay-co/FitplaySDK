using UnityEngine;

namespace MotionCapture.StandTravelModel.Editor.Core
{
    public enum MotionMode
    {
        Stand = 0,
        Travel
    }

    public class StandTravelAnchorController
    {
        private Vector3 initialPosition;
        private GameObject travelFollowPoint;
        public GameObject TravelFollowPoint => travelFollowPoint;
        
        private GameObject standFollowPoint;

        public GameObject StandFollowPoint => standFollowPoint;

        public StandTravelAnchorController(Vector3 initialPosition)
        {
            this.initialPosition = initialPosition;

            this.travelFollowPoint = new GameObject("TravelFollowPoint");
            travelFollowPoint.transform.position = initialPosition;

            this.standFollowPoint = new GameObject("StandFollowPoint");
            standFollowPoint.transform.position = initialPosition;
        }

        ~StandTravelAnchorController()
        {
            UnityEngine.Object.Destroy(travelFollowPoint);
            UnityEngine.Object.Destroy(standFollowPoint);
        }

        public void MoveTravelPoint(Vector3 deltaDistance)
        {
            travelFollowPoint.transform.position += deltaDistance;
        }

        public void TurnControlPoints(Quaternion deltaAngle)
        {
            standFollowPoint.transform.rotation *= deltaAngle;
            travelFollowPoint.transform.rotation *= deltaAngle;
        }

    }
}