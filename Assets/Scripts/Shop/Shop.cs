using UnityEngine;

public class Shop : MonoBehaviour
{
    private ShopCategory shopCategory;
    public ShopCategory ShopCategory => shopCategory;

    private void Awake()
    {
        shopCategory = new ShopCategory();
    }
}
