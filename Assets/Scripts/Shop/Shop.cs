using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
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

    private void Awake()
    {
        shopCategory = new ShopCategory();
        shopCategory.Init();
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
