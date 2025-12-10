using UnityEngine;

public class SlashVFX : BaseVFX
{
    public override void Play(Vector3 position)
    {
        base.Play(position);
        Debug.Log("Slash effect triggered!");
    }
}

public class ExplosionVFX : BaseVFX
{
    public override void Play(Vector3 position)
    {
        base.Play(position);
    }
}