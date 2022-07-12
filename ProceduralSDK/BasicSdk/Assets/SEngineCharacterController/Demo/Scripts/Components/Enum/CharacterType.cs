namespace SEngineCharacterController
{
    public enum CharacterType
    {
        /// <summary>
        /// 自己
        /// </summary>
        Master,
        /// <summary>
        /// 敌人
        /// </summary>
        Enemy,
        /// <summary>
        /// 队友
        /// </summary>
        Teammate,
        /// <summary>
        /// npc
        /// </summary>
        Npc,
    }
    
    public enum CharacterState
    {
        Idle = 0,
        Die,
    }
}