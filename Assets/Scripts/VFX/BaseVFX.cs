using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BaseVFX : MonoBehaviour
{
    protected ParticleSystem ps;
    protected float lifeTime = 0.5f;
    protected string id;

    protected virtual void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public virtual void Initialize(VFXData data)
    {
        id = data != null ? data.vfxID : id;
        lifeTime = data != null ? data.lifeTime : lifeTime;
    }

    public virtual void Play(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        if (ps != null)
        {
            ps.Clear();
            ps.Play();
        }
        CancelInvoke(nameof(Deactivate));
        Invoke(nameof(Deactivate), lifeTime);
    }

    public virtual void Stop()
    {
        if (ps != null) ps.Stop();
        Deactivate();
    }

    protected virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }
}