using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour
{
    [SerializeField] private Button buyButton;

    private ShopCategory shopCategory;
    public ShopCategory ShopCategory => shopCategory;

    private List<ItemCategory> currentWeaponInShop = new();
    private List<ItemCategory> currentChestplateInShop = new();
    private List<ItemCategory> currentRingInShop = new();
    private List<ItemCategory> currentNecklaceInShop = new();
    private List<ItemCategory> currentSpecialInShop = new();
    private List<ItemCategory> currentOtherInShop = new();
    public List<ItemCategory> CurrentWeaponInShop => currentWeaponInShop;
    public List<ItemCategory> CurrentChestplateInShop => currentChestplateInShop;
    public List<ItemCategory> CurrentRingInShop => currentRingInShop;
    public List<ItemCategory> CurrentNecklaceInShop => currentNecklaceInShop;
    public List<ItemCategory> CurrentSpecialInShop => currentSpecialInShop;
    public List<ItemCategory> CurrentOtherInShop => currentOtherInShop;

    private ItemStock currentItemSelected;
    private ShopCategories currentCategory;
    public Action<ItemStock> OnCurrentItemChanged;
    public Action OnItemBuyed;

    private IPlayerRuntime playerRuntime;

    public void BindPlayer(IPlayerRuntime data)
    {
        playerRuntime = data;
    }

    private void Awake()
    {
        shopCategory = new ShopCategory();
        shopCategory.Init();

        buyButton.onClick.AddListener(OnBuyClicked);
    }

    public void SelectItem(ItemStock itemStock)
    {
        currentItemSelected = itemStock;
        OnCurrentItemChanged?.Invoke(currentItemSelected);


        Image buttonImage = buyButton.GetComponent<Image>();
        if (itemStock != null)
        {
            buyButton.interactable = true;
            buttonImage.color = Color.white;
        }
        else
        {
            buyButton.interactable = false;
            buttonImage.color = Color.gray;
        }
    }

    public void SetCurrentCategory(ShopCategories category)
    {
        currentCategory = category;
    }

    private void OnBuyClicked()
    {
        Inventory playerInventory = playerRuntime.PlayerInventory;
        if (playerInventory != null)
        {
            if (currentItemSelected != null)
            {
                ItemCategory itemData = currentItemSelected.ItemData;
                if (itemData != null)
                {
                    int price = itemData.price;
                    if (playerRuntime.CurrencyWallet.TrySpend(itemData.currencyType, price, true))
                    {
                        bool added = playerInventory.AddItem(itemData.ItemData, 1);
                        if (added)
                        {
                            Debug.Log($"Purchased item {itemData.ItemData.itemName} for {price} {itemData.currencyType}.");
                            itemData.stock--;
                            
                            if (itemData.stock <= 0)
                            {
                                switch (currentCategory)
                                {
                                    case ShopCategories.Weapon:
                                        currentWeaponInShop.Remove(itemData);
                                        break;
                                    case ShopCategories.Chestplate:
                                        currentChestplateInShop.Remove(itemData);
                                        break;
                                    case ShopCategories.Ring:
                                        currentRingInShop.Remove(itemData);
                                        break;
                                    case ShopCategories.Necklace:
                                        currentNecklaceInShop.Remove(itemData);
                                        break;
                                    case ShopCategories.Special:
                                        currentSpecialInShop.Remove(itemData);
                                        break;
                                    case ShopCategories.Other:
                                        currentOtherInShop.Remove(itemData);
                                        break;
                                }
                            }

                            OnItemBuyed?.Invoke();
                        }
                        else
                        {
                            Debug.LogWarning("Could not add item to inventory. Purchase failed.");
                            playerRuntime.CurrencyWallet.Add(itemData.currencyType, price, true);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Not enough currency to purchase item.");
                    }
                }
            }
        }
    }

    public void InitCategory()
    {
        if (shopCategory == null)
        {
            shopCategory = new ShopCategory();
            shopCategory.Init();
        }

        currentWeaponInShop.Clear();
        currentChestplateInShop.Clear();
        currentRingInShop.Clear();
        currentNecklaceInShop.Clear();
        currentSpecialInShop.Clear();
        currentOtherInShop.Clear();

        if (shopCategory.weaponCategoryData.itemCategories != null)
        {
            foreach (var itemCategory in shopCategory.weaponCategoryData.itemCategories)
            {
                float randomIndex = Random.Range(0f, 1f);
                if (randomIndex <= itemCategory.rate)
                {
                    currentWeaponInShop.Add(itemCategory);
                }
            }
        }   

        if (shopCategory.chestplateCategoryData.itemCategories != null)
        {
            foreach (var itemCategory in shopCategory.chestplateCategoryData.itemCategories)
            {
                float randomIndex = Random.Range(0f, 1f);
                if (randomIndex <= itemCategory.rate)
                {
                    currentChestplateInShop.Add(itemCategory);
                }
            }
        }
           
        if (shopCategory.ringCategoryData.itemCategories != null)
        {
            foreach (var itemCategory in shopCategory.ringCategoryData.itemCategories)
            {
                float randomIndex = Random.Range(0f, 1f);
                if (randomIndex <= itemCategory.rate)
                {
                    currentRingInShop.Add(itemCategory);
                }
            }
        }
        
        if (shopCategory.necklaceCategoryData.itemCategories != null)
        {
            foreach (var itemCategory in shopCategory.necklaceCategoryData.itemCategories)
            {
                float randomIndex = Random.Range(0f, 1f);
                if (randomIndex <= itemCategory.rate)
                {
                    currentNecklaceInShop.Add(itemCategory);
                }
            }
        }
        
        if (shopCategory.specialCategoryData.itemCategories != null)
        {
            foreach (var itemCategory in shopCategory.specialCategoryData.itemCategories)
            {
                float randomIndex = Random.Range(0f, 1f);
                if (randomIndex <= itemCategory.rate)
                {
                    currentSpecialInShop.Add(itemCategory);
                }
            }
        }

        if (shopCategory.otherCategoryData.itemCategories != null)
        {
            foreach (var itemCategory in shopCategory.otherCategoryData.itemCategories)
            {
                float randomIndex = Random.Range(0f, 1f);
                if (randomIndex <= itemCategory.rate)
                {
                    currentOtherInShop.Add(itemCategory);
                }
            }
        }
    }
}
