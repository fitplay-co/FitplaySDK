namespace SEngineBasic
{
    public struct GroundLocationControl : OSApplicationBase
    {
        public string type;
        public string feature_id;
        public string action;
        public GroundLocationControl(EOSActionType actionType)
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