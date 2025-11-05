using NUnit.Framework;
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
    [SerializeField] private GameObject itemStockPrefab;
    [SerializeField] private Transform itemStockContainer;

    private IPlayerRuntime playerRuntime;

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
    }

    public void BindData(IPlayerRuntime data)
    {
        playerRuntime = data;
        Initialize();
    }

    public void ChangeCategory(ShopCategories category)
    {
        foreach (Transform child in itemStockContainer)
        {
            Destroy(child.gameObject);
        }

        Category categoryToChange = new Category();
        switch  (category)
        {
            case ShopCategories.Weapon:
                categoryToChange = shop.ShopCategory.weaponCategoryData;
                break;
            case ShopCategories.Chestplate:
                categoryToChange = shop.ShopCategory.chestplateCategoryData;
                break;
        }

        List<ItemCategory> itemToAdd = new();
        int count = categoryToChange.itemCategories.Count;

        for (int i = 0;  i < count; i++)
        {
            float randomIndex = Random.Range(0f, 1f);
            if (randomIndex <= categoryToChange.itemCategories[i].rate)
            {
                itemToAdd.Add(categoryToChange.itemCategories[i]);
            }
        }

        foreach (ItemCategory itemCategory in itemToAdd)
        {
            GameObject itemStockGO = Instantiate(itemStockPrefab, itemStockContainer);
            ItemStock itemStock = itemStockGO.GetComponent<ItemStock>();
            itemStock.SetData(itemCategory);
        }
    }
}
