using MotionCaptureBasic.OSConnector;

namespace FK
{
    public interface IFKPoseModel
    {
        void SetEFKTypes(params EFKType[] eFKTypes);
        void SetEnable(bool active);
    }
}