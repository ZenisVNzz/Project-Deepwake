using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "InitConfigTask", menuName = "StartupSystem/InitConfigTask")]
public class InitConfigTask : StartupTask
{
    public override bool HasTimeout { get { return true; } }

    public override async Task<bool> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        IServiceRegistry SR = new ServiceRegistry();

        AsyncOperationHandle<ConfigList> handle = Addressables.LoadAssetAsync<ConfigList>("ConfigList");
        ConfigList configList = await handle.Task;

        IConfigLoader loader = new ConfigLoader();

        foreach (var config in configList.Configs)
        {
            var type = config.GetType();

            string fileName = type.Name + ".json";
            JsonReader reader = new JsonReader(Application.persistentDataPath);
            string json = reader.Read(fileName);

            JsonUtility.FromJsonOverwrite(json, config);

            var registryMethod = typeof(IServiceRegistry).GetMethod("Register");
            var genericRegister = registryMethod.MakeGenericMethod(type);
            genericRegister.Invoke(SR, new object[] { config });
        }

        var configManager = new ConfigManager(SR);
        await configManager.InitAsync(serviceRegistry, ct);

        return true;
    }
}
