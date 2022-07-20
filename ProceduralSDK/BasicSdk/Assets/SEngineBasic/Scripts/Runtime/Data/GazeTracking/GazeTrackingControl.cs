namespace SEngineBasic
{
    public struct GazeTrackingControl : OSApplicationBase
    {
        public string type;
        public string feature_id;
        public string action;
        public GazeTrackingControl(EOSActionType actionType)
        {
            type = "application_control";
            feature_id = "ground_location";
            switch (actionType)
            {
                case EOSActionType.Subscribe:
                    action = "subscribe";
                    break;
                default:
                    action = "reset";
                    break;
            }
        }
    }
}