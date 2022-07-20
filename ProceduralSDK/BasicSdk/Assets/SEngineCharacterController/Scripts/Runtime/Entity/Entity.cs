using System;
using System.Collections.Generic;

namespace SEngineCharacterController
{
    public partial class Entity : IDisposable
    {
        public int Id {  get; private set;}
        
        public void SetId(int id)
        {
            Id = id;
        }
        
        private Dictionary<Type, Component> components = new Dictionary<Type, Component>();
        
        public Dictionary<Type, Component> Components
        {
            get
            {
                return components;
            }
        }
        
        protected virtual void OnInit()
        {
            
        }

        public virtual void OnInit(object initData)
        {
            
        }
        
        public T AddComponent<T>() where T : Component
        {
            var type = typeof(T);
            if (Components.TryGetValue(type, out var component)) return component as T;
            component = Activator.CreateInstance<T>();
            component.SetEntity(this);
            component.OnInit();
            components.Add(type, component);

            return (T) component;
        }
        
        public T AddComponent<T>(object initData) where T : Component
        {
            var type = typeof(T);
            if (Components.TryGetValue(type, out var component)) return component as T;
            component = Activator.CreateInstance<T>();
            component.SetEntity(this);
            component.OnInit(initData);
            components.Add(type, component);

            return (T) component;
        }

        public T AddComponent<T>(object initData, object initData2) where T : Component
        {
            var type = typeof(T);
            if (Components.TryGetValue(type, out var component)) return component as T;
            component = Activator.CreateInstance<T>();
            component.SetEntity(this);
            component.OnInit(initData, initData2);
            components.Add(type, component);

            return (T) component;
        }
        
        public T AddComponent<T>(object initData, object initData2, object initData3) where T : Component
        {
            var type = typeof(T);
            if (Components.TryGetValue(type, out var component)) return component as T;
            component = Activator.CreateInstance<T>();
            component.SetEntity(this);
            component.OnInit(initData, initData2, initData3);
            components.Add(type, component);

            return (T) component;
        }
        
        public T AddComponent<T>(object initData, object initData2, object initData3, object initData4) where T : Component
        {
            var type = typeof(T);
            if (Components.TryGetValue(type, out var component)) return component as T;
            component = Activator.CreateInstance<T>();
            component.SetEntity(this);
            component.OnInit(initData, initData2, initData3, initData4);
            components.Add(type, component);

            return (T) component;
        }
        
        public void RemoveComponent<T>() where T : Component
        {
            var type = typeof(T);
            if (!components.TryGetValue(type, out var component)) return;
            component.Dispose();
            components.Remove(type);
        }

        public T GetComponent<T>() where T : Component
        {
            var type = typeof(T);
            if (Components.TryGetValue(type, out var component)) return component as T;
            return null;
        }

        public virtual void Dispose()
        {
            foreach (var component in components.Values)
            {
                component.Dispose();
            }
            components.Clear();
        }
    }
}

