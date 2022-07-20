using System;
using SEngineCharacterController;

namespace RootMotion.FinalIK.FitPlayProcedural
{
    public class StateComponent : IDisposable
    {
        public bool IsDisposed { get; private set; }
       
        public virtual void OnInit()
        {
            IsDisposed = false;
        }
        
        public virtual void OnInit(object initData)
        {
            IsDisposed = false;
        }
        
        public virtual void OnInit(object initData, object initData2)
        {
            IsDisposed = false;
        }
        
        public virtual void OnInit(object initData, object initData2, object initData3)
        {
            IsDisposed = false;
        }
        
        public virtual void OnInit(object initData, object initData2, object initData3, object initData4)
        {
            IsDisposed = false;
        }

        public virtual void Dispose()
        {
            IsDisposed = true;
        }
    }
}