using System;

namespace SEngineCharacterController
{
    public class Component : IDisposable
    {
        public Entity Owner { get; private set; }
        public bool IsDisposed { get; private set; }
        public void SetEntity(Entity entity)
        {
            Owner = entity;
        }
        
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