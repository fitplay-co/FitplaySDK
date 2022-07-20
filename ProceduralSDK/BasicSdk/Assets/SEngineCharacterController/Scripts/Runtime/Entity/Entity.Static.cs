using System;
using System.Collections.Generic;

namespace SEngineCharacterController
{
    public partial class Entity
    {
        private static Dictionary<int, Entity> entities = new Dictionary<int, Entity>();

        public static Dictionary<int, Entity> Entities
        {
            get { return entities; }
        }

        public static T Create<T>(int id) where T : Entity
        {
            var entity = NewEntity(typeof(T), id) as T;
            entity.OnInit();
            return entity;
        }

        public static T Create<T>(int id, object initData) where T : Entity
        {
            var entity = NewEntity(typeof(T), id) as T;
            entity.OnInit(initData);
            return entity;
        }

        public static void Remove(int id)
        {
            if (!entities.TryGetValue(id, out var entity)) return;
            entity.Dispose();
            entities.Remove(id);
        }

        public static T Get<T>(int id) where T : Entity
        {
            if (entities.TryGetValue(id, out var entity))
            {
                return entity as T;
            }

            return null;
        }

        private static Entity NewEntity(Type entityType, int id)
        {
            if (!(Activator.CreateInstance(entityType) is Entity entity)) return null;
            
            entity.SetId(id);
            entities.Add(entity.Id, entity);
            return entity;
        }
    }
}
