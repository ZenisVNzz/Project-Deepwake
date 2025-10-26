using System.Collections.Generic;

public interface IServiceRegistry
{
    void RegisterService<T>(T service);
    T GetService<T>();
}

public class ServiceRegistry : IServiceRegistry
{
    private Dictionary<System.Type, object> _services = new();

    public void RegisterService<T>(T service)
    {
        _services[typeof(T)] = service;
    }

    public T GetService<T>()
    {
        if (_services.TryGetValue(typeof(T), out var service))
            return (T)service;
        return default;
    }
}