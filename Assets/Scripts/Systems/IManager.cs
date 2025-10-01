using System.Threading.Tasks;
using UnityEngine;

public interface IManager
{
    Task<bool> InitAsync();
}
