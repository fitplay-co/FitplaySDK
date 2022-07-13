using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Scripts
{
    public class FKSkeletonNode
    {
        public GameObject LineObject;
        public LineRenderer Line;

        public EFKType start;
        public EFKType end;
    }
}