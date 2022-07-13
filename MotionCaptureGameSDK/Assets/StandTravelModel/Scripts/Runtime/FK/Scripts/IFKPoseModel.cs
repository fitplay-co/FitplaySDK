using MotionCaptureBasic.OSConnector;

namespace StandTravelModel.Scripts.Runtime.FK.Scripts
{
    public interface IFKPoseModel
    {
        void SetActiveEFKTypes(params EFKType[] eFKTypes);
        void SetFullBodyEFKTypes();
        void SetEnable(bool active);
        bool IsEnabled();
        void Initialize();
    }
}