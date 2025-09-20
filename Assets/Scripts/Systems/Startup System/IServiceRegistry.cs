using System.ComponentModel.Design;
using UnityEngine;

public interface IServiceRegistry
{
    void Register<TService>(TService service);
    TService Get<TService>();
    TService TryGet<TService>(out TService service);
}
