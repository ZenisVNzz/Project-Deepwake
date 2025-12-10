using UnityEngine;

public class ShopCategory
{
    public Category weaponCategoryData = new Category();
    public Category chestplateCategoryData = new Category();
    public Category ringCategoryData = new Category();
    public Category necklaceCategoryData = new Category();
    public Category specialCategoryData = new Category();
    public Category otherCategoryData = new Category();

    public ShopCategory() { }

    public void Init()
    {
        weaponCategoryData = ResourceManager.Instance.GetAsset<Category>("WeaponCategory");
        chestplateCategoryData = ResourceManager.Instance.GetAsset<Category>("ChestplateCategory");
    }
}
