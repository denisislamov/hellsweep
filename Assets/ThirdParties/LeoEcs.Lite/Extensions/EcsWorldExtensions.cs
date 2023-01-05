using System;

namespace Leopotam.EcsLite.Extensions
{
    public static class EcsWorldExtensions
    {
        public static bool HasEntityAnyComponent(this EcsWorld world, int entityId, Type[] componentTypes)
        {
            foreach (var componentType in componentTypes)
            {
                if (world.GetPoolByType(componentType)?.Has(entityId) == true)
                {
                    return true;
                }
            }

            return false;
        }

        public static void Delete<T>(this EcsWorld world, int entityId) where T:struct
        {
            world.GetPool<T>().Del(entityId);
        }

        public static ref T Add<T>(this EcsWorld world, int entityId) where T : struct
        {
            return ref world.GetPool<T>().Add(entityId);
        }
        
        public static bool Has<T>(this EcsWorld world, int entityId) where T : struct
        {
            return world.GetPool<T>().Has(entityId);
        }
        
        public static ref T GetOrAdd<T>(this EcsWorld world, int entityId) where T : struct
        {
            var pool = world.GetPool<T>();

            if (pool.Has(entityId))
            {
                return ref pool.Get(entityId);
            }
            else
            {
                return ref pool.Add(entityId);
            }
        }
    }
}