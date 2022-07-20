namespace SEngineBasic
{
    /// <summary>
    /// 订阅Fitting数据
    /// </summary>
    public enum EFittingType
    {
        Mirror,
        Camera,
        Dual,
    }
    public struct FittingControl : OSApplicationBase
    {
        public string type;
        public string feature_id;
        public string action;
        public string data;
        public FittingControl(EOSActionType actionType, EFittingType fittingType)
        {
            type = "application_control";
            feature_id = "fitting";
            switch (actionType)
            {
                case EOSActionType.Subscribe:
                    action = "subscribe";
                    break;
                default:
                    action = "release";
                    break;
            }
            switch (fittingType)
            {
                case EFittingType.Camera:
                    data = "camera";
                    break;
                case EFittingType.Mirror:
                    data = "mirror";
                    break;
                default:
                    data = "dual";
                    break;
            }
        }
    }
}