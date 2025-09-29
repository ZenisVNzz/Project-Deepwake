using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public interface IPopupService : IUIService
{
    void Create(string prefabID, string instanceID, LocalizedString content, Action button1, Action button2);
}
