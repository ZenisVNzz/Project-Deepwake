using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StartupTaskList", menuName = "StartupSystem/StartupTaskList")]
public class StartupTaskList : ScriptableObject
{
    public List<StartupTask> TaskList = new List<StartupTask>();
}
