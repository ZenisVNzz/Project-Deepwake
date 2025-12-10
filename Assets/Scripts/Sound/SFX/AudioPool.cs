using UnityEngine;
using System.Collections.Generic;

public class AudioPool : MonoBehaviour
{
    [SerializeField] private AudioObject audioPrefab;
    private Queue<AudioObject> pool = new Queue<AudioObject>();

    public AudioObject Get()
    {
        if (pool.Count > 0)
        {
            var a = pool.Dequeue();
            a.gameObject.SetActive(true);
            return a;
        }

        var newObj = Instantiate(audioPrefab, transform);
        newObj.Init(this);
        return newObj;
    }

    public void ReturnToPool(AudioObject obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);

        Debug.Log("Returned AudioObject to pool");
    }
}