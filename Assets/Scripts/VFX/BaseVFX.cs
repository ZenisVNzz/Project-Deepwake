using UnityEngine;

public abstract class BaseVFX : MonoBehaviour, IVFX, IPoolable
{
    [SerializeField] protected ParticleSystem particle;
    protected float lifeTime;

    public virtual void Initialize(VFXData data)
    {
        lifeTime = data.lifeTime;
    }

    public virtual void Play(Vector3 position)
    {
        transform.position = position;
        particle?.Play();
        Invoke(nameof(Stop), lifeTime);
    }

    public virtual void Stop()
    {
        particle?.Stop();
        gameObject.SetActive(false);
    }

    public void OnSpawned() => particle?.Clear();
    public void OnDespawned() => CancelInvoke();
}