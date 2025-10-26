using System;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeUIServiceRegistry : IServiceRegistry
{
    private Dictionary<Type, object> _serviceData = new Dictionary<Type, object>();

    public void Register<IRuntimeUIService>(IRuntimeUIService service)
    {
        _serviceData[typeof(IRuntimeUIService)] = service;
    }

    public IRuntimeUIService Get<IRuntimeUIService>()
    {
        if (_serviceData.TryGetValue(typeof(IRuntimeUIService), out var service)) { return (IRuntimeUIService)service; }
        throw new InvalidOperationException($"Service {typeof(IRuntimeUIService).Name} not registered");
    }

    public bool TryGet<IRuntimeUIService>(out IRuntimeUIService service)
    {
        if (_serviceData.TryGetValue(typeof(IRuntimeUIService), out var s))
        {
            service = (IRuntimeUIService)s;
            return true;
        }
        service = default(IRuntimeUIService);
        return false;
    }
}
