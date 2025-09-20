using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceRegistry : IServiceRegistry
{
    private Dictionary<Type, object> _serviceData = new Dictionary<Type, object>();

    public void Register<TService>(TService service)
    {
        _serviceData[typeof(TService)] = service;
    }

    public TService Get<TService>()
    {
        if (_serviceData.TryGetValue(typeof(TService), out var service)) { return (TService)service; }
        throw new InvalidOperationException($"Service {typeof(TService).Name} not registered");
    }

    public bool TryGet<TService>(out TService service)
    {
        if (_serviceData.TryGetValue(typeof(TService), out var s))
        {
            service = (TService)s;
            return true;
        }
        service = default(TService);
        return false;
    }
}
