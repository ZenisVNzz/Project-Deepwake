using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ConfigManager : IGameService
{
    private IServiceRegistry _serviceRegistry;

    public ConfigManager(IServiceRegistry serviceRegistry)
    {
        _serviceRegistry = serviceRegistry;
    }

    public async Task<bool> InitAsync(IServiceRegistry services, CancellationToken ct)
    {
        services.Register<ConfigManager>(this);
        await Task.CompletedTask;
        return true;
    }

    public Tservice GetConfig<Tservice>()
    {
        return _serviceRegistry.Get<Tservice>();
    }

    public bool GetConfig<Tservice>(out Tservice service)
    {
        return _serviceRegistry.TryGet<Tservice>(out service);
    }
}
