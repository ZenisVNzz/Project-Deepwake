using UnityEngine;

public class HurtFlash : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Material flashMaterial;
    public DamageFlash damageFlash;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (flashMaterial == null)
        {
            flashMaterial = ResourceManager.Instance.GetAsset<Material>("DamageFlashMaterial");       
        }
        damageFlash = new DamageFlash(spriteRenderer, flashMaterial);

        damageFlash.TriggerFlash();
    }
}
