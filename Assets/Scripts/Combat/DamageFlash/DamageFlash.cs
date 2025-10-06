using System.Threading.Tasks;
using UnityEngine;

public class DamageFlash
{
    private Material _defaultMaterial;
    private Material _flashMaterial;
    private float _flashDuration = 0.15f;
    private float _flashTimer;

    private SpriteRenderer _spriteRenderer;

    public DamageFlash(SpriteRenderer spriteRenderer, Material flahMaterial)
    {
        _spriteRenderer = spriteRenderer;
        _defaultMaterial = spriteRenderer.material;
        _flashMaterial = flahMaterial;
    }

    public void TriggerFlash()
    {
        _spriteRenderer.material = _flashMaterial;
        _flashTimer = _flashDuration;
        _ = UpdateFlash();
    }

    public async Task UpdateFlash()
    {
        while (_flashTimer > 0)
        {
            _flashTimer -= Time.deltaTime;
            if (_flashTimer <= 0)
            {
                _spriteRenderer.material = _defaultMaterial;
            }
            await Task.Yield();
        }
    }
}
