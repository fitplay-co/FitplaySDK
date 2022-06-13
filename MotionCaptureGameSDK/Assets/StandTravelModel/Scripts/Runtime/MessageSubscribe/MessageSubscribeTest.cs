using UnityEngine;
using MotionCaptureBasic;

public class MessageSubscribeTest : MonoBehaviour
{
    [SerializeField] private bool isDebug;
    [SerializeField] private bool connected;

    private void Awake() {
        MotionDataModelHttp.GetInstance().AddConnectEvent(
            () => {
                connected = true;
            }
        );
    }

    private void Update() {
        MotionDataModelHttp.GetInstance().SetDebug(isDebug);
    }

    public void MessagesSubscribe()
    {
        if(connected)
        {
            MotionDataModelHttp.GetInstance().SubscribeGazeTracking();
            MotionDataModelHttp.GetInstance().SubscribeGroundLocation();
            MotionDataModelHttp.GetInstance().SubscribeActionDetection();
        }
    }

    public void MessagesRelease()
    {
        if(connected)
        {
            MotionDataModelHttp.GetInstance().ReleaseGazeTracking();
            MotionDataModelHttp.GetInstance().ReleaseGroundLocation();
            MotionDataModelHttp.GetInstance().ReleaseActionDetection();
        }
    }
}