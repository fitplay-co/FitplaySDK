using System;

namespace SEngineBasic
{
    public struct ImuFPS : OSApplicationBase
    {
        public string type;
        public string feature_id;
        public string action;
        public FPSData data;
        public ImuFPS(int fps)
        {
            type = "application_control";
            feature_id = "imu";
            action = "config";
            data = new FPSData(fps);
        }
        [Serializable]
        public struct FPSData
        {
            public int fps;
            public FPSData(int fps)
            {
                this.fps = fps;
            }
        }
    }
}