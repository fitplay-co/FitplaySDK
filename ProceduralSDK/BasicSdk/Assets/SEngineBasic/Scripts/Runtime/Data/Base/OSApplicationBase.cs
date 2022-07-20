namespace SEngineBasic
{
    /// <summary>
    /// 控制帧类型
    /// </summary>
    public enum EOSActionType
    {
        /// <summary>
        /// 订阅
        /// </summary>
        Subscribe,
        /// <summary>
        /// 取消订阅
        /// </summary>
        Release,
    }
    interface OSApplicationBase { }
}