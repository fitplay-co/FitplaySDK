using UnityEngine;

namespace StandTravelModel.Core
{
    public enum MotionMode
    {
        Stand = 0,
        Travel
    }

    public class AnchorController
    {
        private GameObject travelFollowPoint;
        public GameObject TravelFollowPoint => travelFollowPoint;
        
        private GameObject standFollowPoint;

        public GameObject StandFollowPoint => standFollowPoint;

        public AnchorController(Vector3 initialPosition)
        {
            this.travelFollowPoint = new GameObject("TravelFollowPoint");
            travelFollowPoint.transform.position = initialPosition;

            this.standFollowPoint = new GameObject("StandFollowPoint");
            standFollowPoint.transform.position = initialPosition;
        }

        public void DestroyObject()
        {
            if(travelFollowPoint != null)
            {
                UnityEngine.Object.Destroy(travelFollowPoint);
                travelFollowPoint = null;
            }

            if(standFollowPoint != null)
            {
                UnityEngine.Object.Destroy(standFollowPoint);
                standFollowPoint = null;
            }
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