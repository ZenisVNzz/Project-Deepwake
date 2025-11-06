using UnityEngine;

public class ShopCategory
{
    public Category weaponCategoryData;
    public Category chestplateCategoryData;
    public Category ringCategoryData;
    public Category necklaceCategoryData;
    public Category specialCategoryData;
    public Category otherCategoryData;

    public ShopCategory() { }

    public void Init()
    {
        weaponCategoryData = ResourceManager.Instance.GetAsset<Category>("WeaponCategory");
        chestplateCategoryData = ResourceManager.Instance.GetAsset<Category>("ChestplateCategory");
    }
}
