using System;
using System.Collections.Generic;
using System.Linq;

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

        private Dictionary<Type, Dictionary<object, object>> _services = new();
        private Dictionary<Type, Dictionary<object, Func<object>>> _serviceConstructors = new();

        private object _defaultKey = new ();
            
        private ServiceProvider()
        {

        }

        public void Register<TService>(TService service) where TService : class
        {
            Register(_defaultKey, service);
        }

        public void Register<TService>(object id, TService service)
        {
            var type = typeof(TService);
            
            if (!_services.ContainsKey(type))
            {
                _services[type] = new Dictionary<object, object>();
            }

            _services[type].Add(id, service);
        }

        public void Register<TService>(Func<TService> factory) where TService : class
        {
            Register(_defaultKey, factory);
        }

        public void Register<TService>(object id, Func<TService> factory) where TService : class
        {
            var type = typeof(TService);

            if (!_serviceConstructors.ContainsKey(type))
            {
                _serviceConstructors[type] = new Dictionary<object, Func<object>>();
            }

            _serviceConstructors[type].Add(id, factory);
        }

        public TService Resolve<TService>() where TService : class
        {
            return Resolve<TService>(_defaultKey);
        }

        public TService Resolve<TService>(object id) where TService : class
        {
            var type = typeof(TService);

            if (_services.TryGetValue(type, out var serviceDictionary)
                && serviceDictionary.TryGetValue(id, out var service))
            {
                return (TService)service;
            }

            if (_serviceConstructors.TryGetValue(type, out var factoryDictionary)
                && factoryDictionary.TryGetValue(id, out var factory))
            {
                var newService = (TService)factory();
                factoryDictionary.Remove(factory);

                Register(id, newService);

                return newService;
            }

            throw new InvalidOperationException($"Service of type {typeof(TService)} is not registered.");
        }
    }
}