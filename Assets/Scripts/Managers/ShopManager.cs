using System;
using Ink.Parsed;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

// Handles all Shop background logic & manages/displays Shop UI

public class ShopManager : NetworkBehaviour
{
    public static ShopManager instance;

    [Header("Buy Shop")]
    [SerializeField] private List<GameObject> BuyStock = new();
    [SerializeField] private ShopItem CurrentlyBuyingItem = null;
    [SerializeField] private int buyCount = 1;
    [SerializeField] private bool updateSellShop = false;

    [Header("Sell Shop")]
    [SerializeField] private List<GameObject> SellStock = new();
    [SerializeField] private ShopItem CurrentlySellingItem = null;
    [SerializeField] private GameObject EmptyShopItem;
    [SerializeField] private int sellCount = 1;

    [Header("Shop UI Elements (Main Parent)")]
    [SerializeField] private GameObject ShopUI;

    [Header("Misc. Shop UI Elements")]
    [SerializeField] private Button ExitShopButton;
    [SerializeField] private Button SwapShopButton;

    [Header("Buy Shop UI Elements")]
    [SerializeField] private GameObject BuyShopUI;
    [SerializeField] private GameObject BuyShopItemsDisplay;
    [SerializeField] private TMP_Text BuyCountDisplay;
    [SerializeField] private TMP_Text CostDisplay;
    [SerializeField] private Image DisplayedBuyItem;
    [SerializeField] private TMP_Text GoldDisplay;

    [Header("Sell Shop UI Elements")]
    [SerializeField] private GameObject SellShopUI;
    [SerializeField] private GameObject SellShopItemsDisplay;
    [SerializeField] private Image DisplayedSellItem;
    [SerializeField] private TMP_Text ProfitDisplay;
    [SerializeField] private TMP_Text SellCountDisplay;

    private Player targetPlayer;


    #region Main Shop (Misc. Stuff)
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }

    private void Start() { SetupShops(); }

    public void ExitShop()
    {
        if (targetPlayer.TryGetComponent<PlayerController>(out var controller))
        {
            ShopUI.SetActive(false);

            // I am not entirely sold on this approach (adding/removing listeners)
            // But it seems to work nicely, even in multiplayer, so I'll take it!

            ExitShopButton.onClick.RemoveListener(() => controller.StopDialogue());

            targetPlayer = null;
        }
    }

    public void OpenShop(Player player, bool openSellShop = false)
    {
        if (player.TryGetComponent<PlayerController>(out var controller))
        {
            ShopUI.SetActive(true);

            ExitShopButton.onClick.AddListener(() => controller.StopDialogue());

            // Set Target Player  (do before resetting shop displays)
            targetPlayer = player;

            // Reset Shop Displays
            ResetBuyShopDisplay();
            ResetSellShopDisplay();

            // Guarantees Sell Shop is populated
            updateSellShop = true;
            
            // Open Specified Shop (Buy/Sell)
            if (openSellShop) ShowSellShop();
            else ShowBuyShop();
        }
    }

    private void SetupShops()
    {
        ShopUI.SetActive(true);

        // Populate Buy Shop
        foreach (GameObject item in BuyStock)
        {
            if (item.TryGetComponent(out ShopItem shopItem))
            {
                if (!shopItem.canBuyItem) shopItem.canBuyItem = true;

                shopItem.image.sprite = shopItem.data.Icon;
                shopItem.quantityDisplay.text = "";

                GameObject newItemDisplay = Instantiate(item);
                newItemDisplay.transform.SetParent(BuyShopItemsDisplay.transform);
            }
        }

        ShopUI.SetActive(false);
    }
    
    public void SwapShop()
    {
        if (BuyShopUI.activeSelf) ShowSellShop();
        else ShowBuyShop();
    }
    #endregion


    #region Buy Shop
    public void Buy()
    {
        if (CurrentlyBuyingItem != null && targetPlayer.playerData.gold > CurrentlyBuyingItem.BuyPrice * buyCount)
        {
            targetPlayer.playerData.gold -= CurrentlyBuyingItem.BuyPrice * buyCount;
            for (int i = 0; i < buyCount; i++) targetPlayer.playerInventory.Add("Hotbar", CurrentlyBuyingItem);

            GoldDisplay.text = "Total Gold: " + targetPlayer.playerData.gold.ToString();

            print(buyCount > 1 ? buyCount.ToString() + " items bought!" : "Item bought!");

            buyCount = 1;
            BuyCountDisplay.text = buyCount.ToString();
            CostDisplay.text = CurrentlyBuyingItem.BuyPrice.ToString() + " G";

            updateSellShop = true;
        }
    }

    public void ChangeBuyCount(int value)
    {
        if (CurrentlyBuyingItem != null && (value > 0 || buyCount != 1) && (buyCount + value) * CurrentlyBuyingItem.BuyPrice <= targetPlayer.playerData.gold)
        {
            buyCount += value;
            BuyCountDisplay.text = buyCount.ToString();
            CostDisplay.text = (CurrentlyBuyingItem.BuyPrice * buyCount).ToString() + " G";
        }
    }

    public void ResetBuyShopDisplay()
    {
        CurrentlyBuyingItem = null;
        DisplayedBuyItem.sprite = null;
        buyCount = 1;
        BuyCountDisplay.text = "";
        CostDisplay.text = "";

        BuyShopUI.SetActive(false);
    }

    public void SelectItemToBuy(ShopItem item)
    {
        CurrentlyBuyingItem = item;
        DisplayedBuyItem.sprite = item.data.Icon;
        buyCount = 1;
        BuyCountDisplay.text = "1";
        CostDisplay.text = item.BuyPrice.ToString() + " G";
    }

    public void ShowBuyShop()
    {
        GoldDisplay.text = "Total Gold: " + targetPlayer.playerData.gold.ToString();

        SellShopUI.SetActive(false);
        BuyShopUI.SetActive(true);
    }
    #endregion


    #region Sell Shop
    public void ChangeSellCount(int value)
    {
        if (CurrentlySellingItem != null && (value > 0 || sellCount != 1) && sellCount + value <= CurrentlySellingItem.quantity)
        {
            sellCount += value;
            SellCountDisplay.text = sellCount.ToString();
            ProfitDisplay.text = "Profit: " + (CurrentlySellingItem.data.SellPrice * sellCount).ToString() + " G";
        }
    }

    private void GetStock()
    {
        // Clear Sell Shop stock
        SellStock.Clear();

        // Clear Sell Shop Item Display
        foreach (Transform child in SellShopItemsDisplay.transform) Destroy(child.gameObject);

        // Get items to sell from Hotbar
        Inventory hotbar = targetPlayer.playerInventory.GetInventoryByName("Hotbar");

        // Add Hotbar items to Sell Shop stock
        foreach (Inventory.Slot slot in hotbar.slots)
        {
            Item item = GameManager.instance.itemManager.GetItemByName(slot.itemName);

            if (item != null)
            {
                GameObject newItem = Instantiate(EmptyShopItem);
                if (newItem.TryGetComponent(out ShopItem sItem))
                {
                    sItem.data = item.data;
                    newItem.name = item.name;
                    sItem.image.sprite = item.data.Icon;
                    sItem.quantity = slot.count;

                    if (sItem.quantityDisplay != null)
                    {
                        sItem.quantityDisplay.text = (slot.count < 10 ? '0' + slot.count.ToString() : slot.count.ToString());
                    }

                    // Populate Sell Shop
                    SellStock.Add(newItem);
                    newItem.transform.SetParent(SellShopItemsDisplay.transform);
                }
                else Destroy(newItem);
            }
        }
    }

    public void ResetSellShopDisplay()
    {
        CurrentlySellingItem = null;
        DisplayedSellItem.sprite = null;
        SellCountDisplay.text = "";
        ProfitDisplay.text = "";

        SellShopUI.SetActive(false);
    }

    public void SelectItemToSell(ShopItem item)
    {
        CurrentlySellingItem = item;
        DisplayedSellItem.sprite = item.data.Icon;
        sellCount = 1;
        SellCountDisplay.text = sellCount.ToString();
        ProfitDisplay.text = "Profit: " + item.data.SellPrice.ToString() + " G";
    }

    public void Sell(bool sellAll)
    {
        if (CurrentlySellingItem != null)
        {
            if (sellAll) sellCount = CurrentlySellingItem.quantity;
            
            CurrentlySellingItem.quantity -= sellCount;
            
            int itemIndex = SellStock.FindIndex(item => item == CurrentlySellingItem.gameObject);
            
            if (itemIndex > 8) targetPlayer.playerInventory.Remove("Inventory", itemIndex, sellCount);
            else if (itemIndex != -1) targetPlayer.playerInventory.Remove("Hotbar", itemIndex, sellCount);
            
            targetPlayer.playerData.gold += CurrentlySellingItem.SellPrice * sellCount;
            
            print(sellCount > 1 ? sellCount.ToString() + " items sold!" : "Item sold!");
            
            if (CurrentlySellingItem.quantity == 0)
            {
                updateSellShop = true;
                ShowSellShop();
            }
            else
            {
                sellCount = 1;
                SellCountDisplay.text = sellCount.ToString();
                CurrentlySellingItem.quantityDisplay.text = (CurrentlySellingItem.quantity < 10 ? '0' + CurrentlySellingItem.quantity.ToString() : CurrentlySellingItem.quantity.ToString());
                ProfitDisplay.text = "Profit: " + CurrentlySellingItem.data.SellPrice.ToString() + " G";
            }
        }
    }

    public void ShowSellShop()
    {
        if (updateSellShop == true)
        {
            GetStock();
            ResetSellShopDisplay();

            updateSellShop = false;
        }

        BuyShopUI.SetActive(false);
        SellShopUI.SetActive(true);
    }
    #endregion
}
