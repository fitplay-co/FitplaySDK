using UnityEngine;
using MotionCaptureBasic.OSConnector;

namespace FK
{
    public class FKSkeletonNode
    {
        public GameObject LineObject;
        public LineRenderer Line;

        public EFKType start;
        public EFKType end;
    }
}