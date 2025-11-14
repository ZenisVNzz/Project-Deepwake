using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour, IRuntimeUIService
{
    [SerializeField] private GameObject ShopUIObject;
    [SerializeField] private CurrencyUI CurrencyUI;

    [SerializeField] private Button weaponCategoryButton;
    [SerializeField] private Button chestplateCategoryButton;
    [SerializeField] private Button ringCategoryButton;
    [SerializeField] private Button necklaceCategoryButton;
    [SerializeField] private Button specialCategoryButton;
    [SerializeField] private Button otherCategoryButton;

    [SerializeField] private Shop shop;
    public Shop Shop => shop;

    [SerializeField] private GameObject itemStockPrefab;
    [SerializeField] private Transform itemStockContainer;

    private IPlayerRuntime playerRuntime;
    private ShopCategories currentCategory;

    private void Awake()
    {
        UIManager.Instance.RuntimeUIServiceRegistry.Register(this);
        weaponCategoryButton.onClick.AddListener(() => ChangeCategory(ShopCategories.Weapon));
        chestplateCategoryButton.onClick.AddListener(() => ChangeCategory(ShopCategories.Chestplate));
        ringCategoryButton.onClick.AddListener(() => ChangeCategory(ShopCategories.Ring));
        necklaceCategoryButton.onClick.AddListener(() => ChangeCategory(ShopCategories.Necklace));
        specialCategoryButton.onClick.AddListener(() => ChangeCategory(ShopCategories.Special));
        otherCategoryButton.onClick.AddListener(() => ChangeCategory(ShopCategories.Other));
    }

    public void Initialize()
    {
        CurrencyUI.Bind(playerRuntime);
        shop.BindPlayer(playerRuntime);
        shop.OnItemBuyed += UpdateUI;
    }

    public void Show()
    {
        if (playerRuntime == null)
        {
            Debug.LogError("ShopUI: playerRuntime is null, cannot initialize");
            return;
        }

        ShopUIObject.SetActive(true);
        ChangeCategory(ShopCategories.Weapon);
    }

    public void Hide()
    {
        ShopUIObject.SetActive(false);
    }

    public void UpdateUI()
    {
        ChangeCategory(currentCategory);
    }

    public void BindData(IPlayerRuntime data)
    {
        playerRuntime = data;
        Initialize();
    }

    public void ChangeCategory(ShopCategories category)
    {
        shop.SelectItem(null);
        shop.SetCurrentCategory(category);

        foreach (Transform child in itemStockContainer)
        {
            Destroy(child.gameObject);
        }

        List<ItemCategory> itemToAdd = new();
        switch  (category)
        {
            case ShopCategories.Weapon:
                itemToAdd = shop.CurrentWeaponInShop;
                break;
            case ShopCategories.Chestplate:
                itemToAdd = shop.CurrentChestplateInShop;
                break;
        }

        foreach (ItemCategory itemCategory in itemToAdd)
        {
            GameObject itemStockGO = Instantiate(itemStockPrefab, itemStockContainer);
            ItemStock itemStock = itemStockGO.GetComponent<ItemStock>();
            itemStock.SetData(itemCategory, shop);
        }

        currentCategory = category;
    }
}
