using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class StartupErrorHandler
{
    private const string TableName = "UI";
    private const string FallbackKey = "UI_GENERIC_STARTUP_ERROR";

    public void ThrowError(string errorId)
    {
        var initOp = LocalizationSettings.InitializationOperation;
        if (!initOp.IsDone)
        {
            initOp.Completed += _ => ShowError(errorId);
        }
        else
        {
            ShowError(errorId);
        }
    }

    private void ShowError(string errorId)
    {
        string dynamicKey = $"UI_{errorId}";

        StringTable table = LocalizationSettings.StringDatabase.GetTable(TableName);
        LocalizedString localized;

        if (table == null)
        {
            Debug.LogWarning($"[Localization] StringTable '{TableName}' is null. Fallback '{FallbackKey}'.");
            localized = new LocalizedString(TableName, FallbackKey);
        }
        else
        {
            var entry = table.GetEntry(dynamicKey);
            if (entry == null)
            {
                Debug.LogWarning($"[Localization] Entry '{dynamicKey}' is null. Fallback '{FallbackKey}'.");
                localized = new LocalizedString(TableName, FallbackKey);
            }
            else
            {
                localized = new LocalizedString(TableName, dynamicKey);
            }
        }

        UIManager.Instance.GetPopupService().Create("100001", errorId, localized);
    }
}
