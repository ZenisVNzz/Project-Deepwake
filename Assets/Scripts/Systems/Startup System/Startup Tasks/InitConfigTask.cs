using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "InitConfigTask", menuName = "StartupSystem/InitConfigTask")]
public class InitConfigTask : StartupTask
{
    public override bool HasTimeout { get { return true; } }

    public override async Task<StartupTaskResult> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        IServiceRegistry SR = new ServiceRegistry();

        ConfigList configList = ResourceManager.Instance.GetAsset<ConfigList>("ConfigList");

        IConfigLoader loader = new ConfigLoader();

        foreach (var config in configList.Configs)
        {           
            var type = config.GetType();

            string fileName = type.Name + ".json";
            Debug.Log($"[ConfigReader] Reading {fileName}");

            try
            {
                loader.Load(config);

                var registryMethod = typeof(IServiceRegistry).GetMethod("Register");
                var genericRegister = registryMethod.MakeGenericMethod(type);
                genericRegister.Invoke(SR, new object[] { config });

                Debug.Log($"[ConfigReader] Reading {fileName} successfully");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[ConfigReader] Reading {fileName} failed\nException: {ex}");
                return StartupTaskResult.Fail("CONFIG_LOAD_FAILED", $"Failed to load config: {fileName}");
            }        
        }

        var configManager = new ConfigManager(SR);
        await configManager.InitAsync(serviceRegistry, ct);

        EventManager.Instance.Trigger("UI_NextProgress");
        return StartupTaskResult.Ok();
    }
}
