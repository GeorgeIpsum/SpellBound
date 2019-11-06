﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace WeWereBound
{
    public class Pooler
    {
        internal Dictionary<Type, Queue<Entity>> Pools { get; private set; }

        public Pooler()
        {
            Pools = new Dictionary<Type, Queue<Entity>>();

            foreach (var type in Assembly.GetEntryAssembly().GetTypes())
            {
                if (type.GetCustomAttributes(typeof(Pooled), false).Length > 0)
                {
                    if (!typeof(Entity).IsAssignableFrom(type)) throw new Exception($"Type '{type.Name}' cannot be Pooled because it doesn't derive from Entity");
                    else if (type.GetConstructor(Type.EmptyTypes) == null) throw new Exception($"Type '{type.Name}' cannot be Pooled because it doesn't have a parameterless constructor");
                    else Pools.Add(type, new Queue<Entity>());
                }
            }
        }

        public T Create<T>() where T : Entity, new()
        {
            if (!Pools.ContainsKey(typeof(T))) return new T();

            var queue = Pools[typeof(T)];
            if (queue.Count == 0) return new T();
            else return queue.Dequeue() as T;
        }

        internal void EntityRemoved(Entity entity)
        {
            var type = entity.GetType();
            if (Pools.ContainsKey(type)) Pools[type].Enqueue(entity);
        }

        public void Log()
        {
            if (Pools.Count == 0) GameEngine.Commands.Log("No Entity types are marked as Pooled!");

            foreach (var kv in Pools)
            {
                string output = $"{kv.Key.Name}:{kv.Value.Count}";
                GameEngine.Commands.Log(output);
            }
        }
    }

    public class Pooled : Attribute { }
}
