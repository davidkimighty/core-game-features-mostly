using System;
using System.Collections.Generic;

namespace BroCollie.Util
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();

        public static void Register<T>(T serviceInstance)
        {
            if (serviceInstance == null) return;

            Type serviceType = typeof(T);
            _services[serviceType] = serviceInstance;
        }

        public static void Unregister<T>()
        {
            Type serviceType = typeof(T);
            _services.Remove(serviceType);
        }

        public static T Get<T>()
        {
            Type serviceType = typeof(T);
            if (_services.TryGetValue(serviceType, out object instance))
            {
                if (instance != null)
                    return (T)instance;
                Unregister<T>();
            }
            throw new InvalidOperationException($"[ServiceLocator] Service '{serviceType.Name}' not registered.");
        }

        public static bool TryGet<T>(out T service)
        {
            service = default;
            Type serviceType = typeof(T);
            if (_services.TryGetValue(serviceType, out object instance))
            {
                if (instance != null)
                {
                    service = (T)instance;
                    return true;
                }
                Unregister<T>();
            }
            return false;
        }

        public static bool Contains<T>()
        {
            return _services.ContainsKey(typeof(T));
        }

        public static void ClearServices()
        {
            _services.Clear();
        }
    }
}
