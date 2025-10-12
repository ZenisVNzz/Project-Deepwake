using System;
using UnityEngine;

public interface IStateHandler
{
    void UpdateState();
    public void Register(string eventName, Action listener);
}
