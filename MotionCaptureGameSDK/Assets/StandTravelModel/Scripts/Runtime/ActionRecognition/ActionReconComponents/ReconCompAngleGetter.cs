using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconComponents
{
    public class ReconCompAngleGetter
    {
        private GameKeyPointsType pointFor;
        private GameKeyPointsType pointMid;
        private GameKeyPointsType pointBak;

        public ReconCompAngleGetter(GameKeyPointsType pointFor, GameKeyPointsType pointMid, GameKeyPointsType pointBak)
        {
            this.pointFor = pointFor;
            this.pointMid = pointMid;
            this.pointBak = pointBak;
        }

        public float GetAngle(List<Vector3> keyPoints)
        {
            var posFor = GetKeyPoint(pointFor, keyPoints);
            var posMid = GetKeyPoint(pointMid, keyPoints);
            var posBak = GetKeyPoint(pointBak, keyPoints);

            var dirMidToFor = GetDirectMidToFor(posMid, posFor);
            var dirBakToMid = GetDirectBakToMid(posBak, posMid);

            return Vector3.Angle(dirMidToFor, dirBakToMid);
        }

        private Vector3 GetKeyPoint(GameKeyPointsType pointsType, List<Vector3> keyPoints)
        {
            return keyPoints[(int)pointsType];
        }

        protected virtual Vector3 GetDirectMidToFor(Vector3 posMid, Vector3 posFor)
        {
            return posFor - posMid;
        }

        protected virtual Vector3 GetDirectBakToMid(Vector3 posBak, Vector3 posMid)
        {
            return posMid - posBak;
        }
    }
}