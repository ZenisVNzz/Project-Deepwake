using System;
using System.Threading;
using System.Threading.Tasks;

namespace Deepwake.NetworkSystem
{
    public class NetworkManager : IGameService
    {
        private IServiceRegistry _serviceRegistry;

        public NetworkManager(IServiceRegistry serviceRegistry)
        {
            _serviceRegistry = serviceRegistry;
        }

        public async Task<bool> InitAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
        {
            serviceRegistry.Register<NetworkManager>(this);
            await Task.CompletedTask;
            return true;
        }

        public Tservice GetService<Tservice>()
        {
            return _serviceRegistry.Get<Tservice>();
        }

        public bool GetService<Tservice>(out Tservice service)
        {
            return _serviceRegistry.TryGet<Tservice>(out service);
        }
    }
}
