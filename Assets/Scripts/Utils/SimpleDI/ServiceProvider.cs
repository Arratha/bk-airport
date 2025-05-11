using System;
using System.Collections.Generic;

namespace Utils.SimpleDI
{
    public class ServiceProvider
    {
        public static ServiceProvider instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceProvider();
                }

                return _instance;
            }
        }

        private static ServiceProvider _instance;

        private Dictionary<Type, object> _services = new();
        private Dictionary<Type, Func<object>> _serviceConstructors = new();

        private ServiceProvider()
        {

        }

        public void Register<TService>(TService service) where TService : class
        {
            _services.Add(typeof(TService), service);
        }

        public void Register<TService>(Func<TService> factory) where TService : class
        {
            _serviceConstructors.Add(typeof(TService), factory);
        }

        public TService Resolve<TService>() where TService : class
        {
            var type = typeof(TService);

            if (_services.TryGetValue(type, out var service))
            {
                return (TService)service;
            }

            if (_serviceConstructors.TryGetValue(type, out var factory))
            {
                var newService = (TService)factory();

                _services.Add(type, newService);
                _serviceConstructors.Remove(type);

                return newService;
            }

            throw new InvalidOperationException($"Service of type {typeof(TService)} is not registered.");
        }
    }
}