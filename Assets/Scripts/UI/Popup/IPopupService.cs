using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPopupService : IUIService
{
    void Create(string prefabID, string instanceID, string content, Action button1, Action button2);
}
