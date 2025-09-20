using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StartupTaskList", menuName = "StartupSystem/StartupTaskList")]
public class StartupTaskList : ScriptableObject
{
    List<IStartupTask> taskList;
}
