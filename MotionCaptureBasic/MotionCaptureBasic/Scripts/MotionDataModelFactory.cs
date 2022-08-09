using System;
using MotionCaptureBasic.Interface;

namespace MotionCaptureBasic
{
    [Serializable]
    public enum MotionDataModelType
    {
        Http = 0,
        Cpp,
        Mobile
    }

    public class MotionDataModelFactory
    {
        public static IMotionDataModel Create(MotionDataModelType type)
        {
            switch (type)
            {
                case MotionDataModelType.Http:
                    return MotionDataModelHttp.GetInstance();
                case MotionDataModelType.Cpp:
                    return null;
                case MotionDataModelType.Mobile:
                    return MotionDataModelMobile.GetInstance();
                default:
                    return null;
            }
        }
    }
}