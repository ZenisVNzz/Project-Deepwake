using System.Collections;
using UnityEngine;

public interface ICoroutineRunner
{
    void RunCoroutine(IEnumerator coroutine);
}
