using MotionCaptureBasic.OSConnector;

namespace FK
{
    public interface IFKPoseModel
    {
        void SetActiveEFKTypes(params EFKType[] eFKTypes);
        void SetEnable(bool active);
        bool IsEnabled();
        void Initialize();
    }
}