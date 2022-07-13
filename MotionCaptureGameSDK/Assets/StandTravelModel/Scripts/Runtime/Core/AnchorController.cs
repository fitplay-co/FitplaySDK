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
        
        private GameObject travelLookAtPoint;
        public GameObject TravelLookAtPoint => travelLookAtPoint;
        
        private GameObject standLookAtPoint;
        public GameObject StandLookAtPoint => standLookAtPoint;

        public AnchorController(Vector3 initialPosition)
        {
            this.travelFollowPoint = new GameObject("TravelFollowPoint");
            travelFollowPoint.transform.position = initialPosition;

            this.standFollowPoint = new GameObject("StandFollowPoint");
            standFollowPoint.transform.position = initialPosition;
            
            this.travelLookAtPoint = new GameObject("TravelLookAtPoint");
            travelLookAtPoint.transform.position = initialPosition;

            this.standLookAtPoint = new GameObject("StandLookAtPoint");
            standLookAtPoint.transform.position = initialPosition;
            
            travelLookAtPoint.transform.SetParent(travelFollowPoint.transform);
            standLookAtPoint.transform.SetParent(standFollowPoint.transform);
        }

        public void DestroyObject()
        {
            if(travelLookAtPoint != null)
            {
                UnityEngine.Object.Destroy(travelLookAtPoint);
                travelLookAtPoint = null;
            }

            if(standLookAtPoint != null)
            {
                UnityEngine.Object.Destroy(standLookAtPoint);
                standLookAtPoint = null;
            }
            
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