using System;

namespace SEngineCharacterController
{
    public class Launcher
    {
        private static Launcher instance;

        public static Launcher Instance
        {
            get
            {
                if (instance == null) instance = new Launcher();
                return instance;
            }
        }

        private Action<float> _tick;
        private Action<float> _fixedTick;
        private Action<float> _lateTick;

        public void OnTick(float dt)
        {
            _tick?.Invoke(dt);
        }

        public void OnFixedTick(float dt)
        {
            _fixedTick?.Invoke(dt);
        }

        public void OnLateTick(float dt)
        {
            _lateTick?.Invoke(dt);
        }

        #region 注册和取消注册
        public void RegisterTick(Action<float> tick)
        {
            if(tick != null) _tick += tick;
        }
        
        public void RegisterFixedTick(Action<float> fixedTick)
        {
            if(fixedTick != null) _fixedTick += fixedTick;
        }
        
        public void RegisterLateTick(Action<float> lateTick)
        {
            if(lateTick != null) _lateTick += lateTick;
        }
        
        public void UnRegisterTick( Action<float> tick)
        {
            if (tick != null) _tick -= tick;
        }
        
        public void UnRegisterFixedTick(Action<float> fixedTick)
        {
            if (fixedTick != null) _fixedTick -= fixedTick;
        }
        
        public void UnRegisterLateTick(Action<float> lateTick)
        {
            if (lateTick != null) _lateTick -= lateTick;
        }
        #endregion
    }
}