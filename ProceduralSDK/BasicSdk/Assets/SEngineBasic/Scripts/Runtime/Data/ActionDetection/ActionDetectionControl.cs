namespace SEngineBasic
{
    public struct ActionDetectionControl: OSApplicationBase
    {
        public string type;
        public string feature_id;
        public string action;

        public ActionDetectionControl(EOSActionType actionType)
        {
            type = "application_control";
            feature_id = "action_detection";
            switch (actionType)
            {
                case EOSActionType.Subscribe:
                    action = "subscribe";
                    break;
                default:
                    action = "release";
                    break;
            }
        }
    }
}