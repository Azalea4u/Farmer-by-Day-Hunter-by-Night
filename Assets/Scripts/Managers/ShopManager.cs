using Ink.Parsed;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

// Handles all Shop background logic & manages/displays Shop UI

public class ShopManager : NetworkBehaviour
{
    public static ShopManager instance;

    [Header("Buy Shop")]
    [SerializeField] private List<GameObject> BuyStock = new();
    [SerializeField] private ShopItem CurrentlyBuyingItem = null;
    [SerializeField] private int buyCount = 1;

    [Header("Sell Shop")]
    [SerializeField] private List<GameObject> SellStock = new();
    [SerializeField] private ShopItem CurrentlySellingItem = null;
    [SerializeField] private GameObject EmptyShopItem;
    [SerializeField] private int sellCount = 1;

    [Header("Shop UI Elements (Main Parent)")]
    [SerializeField] private GameObject ShopUI;

    [Header("Main Shop UI Elements")]
    [SerializeField] private GameObject MainShopUI;
    [SerializeField] private Button ExitShopButton;
    [SerializeField] private Button SwapShopButton;

    [Header("Buy Shop UI Elements")]
    [SerializeField] private GameObject BuyShopUI;
    [SerializeField] private GameObject BuyShopItemsDisplay;
    [SerializeField] private TMP_Text BuyCountDisplay;
    [SerializeField] private TMP_Text CostDisplay;
    [SerializeField] private Image DisplayedItem;
    [SerializeField] private TMP_Text GoldDisplay;

    [Header("Sell Shop UI Elements")]
    [SerializeField] private GameObject SellShopUI;
    [SerializeField] private GameObject SellShopItemsDisplay;

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

        SetupShops();

        ShopUI.SetActive(false);
    }

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

    public void OpenShop(Player player)
    {
        if (player.TryGetComponent<PlayerController>(out var controller))
        {
            ShopUI.SetActive(true);
            if (!MainShopUI.activeSelf) MainShopUI.SetActive(true);

            ExitShopButton.onClick.AddListener(() => controller.StopDialogue());

            // Set Target Player  (do before resetting shop displays)
            targetPlayer = player;

            // Reset Shop Displays
            ResetBuyShopDisplay();
            ResetSellShopDisplay();

            SwapShopButton.gameObject.SetActive(false);
        }
    }

    private void SetupShops()
    {
        // Populate Buy Shop
        foreach (GameObject item in BuyStock)
        {
            if (item.TryGetComponent(out ShopItem shopItem))
            {
                if (!shopItem.canBuyItem) shopItem.canBuyItem = true;
                GameObject newItemDisplay = Instantiate(item);
                newItemDisplay.transform.SetParent(BuyShopItemsDisplay.transform);
                if (shopItem.quantityDisplay != null)
                {
                    shopItem.quantityDisplay.text = "";
                }
            }
        }
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
            targetPlayer.inventoryManager.Add("Hotbar", CurrentlyBuyingItem);

            GoldDisplay.text = "Total Gold: " + targetPlayer.playerData.gold.ToString();

            print("Item bought!");
        }
    }

    public void ChangeBuyCount(int value)
    {
        if (CurrentlyBuyingItem != null && (value > 0 || buyCount != 1))
        {
            buyCount += value;
            BuyCountDisplay.text = buyCount.ToString();
            GoldDisplay.text = "Total Gold: " + targetPlayer.playerData.gold.ToString();
        }
    }

    public void ResetBuyShopDisplay()
    {
        CurrentlyBuyingItem = null;
        DisplayedItem.sprite = null;
        buyCount = 1;
        BuyCountDisplay.text = "";
        CostDisplay.text = "";
        GoldDisplay.text = "Total Gold: " + targetPlayer.playerData.gold.ToString();

        if (BuyShopUI.activeSelf) BuyShopUI.SetActive(false);
    }

    public void SelectItemToBuy(ShopItem item)
    {
        CurrentlyBuyingItem = item;
        DisplayedItem.sprite = item.data.Icon;
        buyCount = 1;
        BuyCountDisplay.text = "1";
        CostDisplay.text = item.BuyPrice.ToString() + " G";
    }

    public void ShowBuyShop()
    {
        if (!SwapShopButton.gameObject.activeSelf) SwapShopButton.gameObject.SetActive(true);

        if (MainShopUI.activeSelf) MainShopUI.SetActive(false);
        if (SellShopUI.activeSelf) SellShopUI.SetActive(false);

        BuyShopUI.SetActive(true);
    }
    #endregion


    #region Sell Shop
    private void GetStock()
    {
        // Clear Sell Shop stock
        SellStock.Clear();

        // Clear Sell Shop Item Display
        foreach (Transform child in SellShopItemsDisplay.transform) Destroy(child.gameObject);

        // Get items to sell from Hotbar
        Inventory hotbar = targetPlayer.inventoryManager.GetInventoryByName("Hotbar");

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

        if (SellShopUI.activeSelf) SellShopUI.SetActive(false);
    }

    public void SelectItemToSell(ShopItem item)
    {
        CurrentlySellingItem = item;
    }

    public void Sell()
    {

    }

    public void ShowSellShop()
    {
        GetStock();        

        if (!SwapShopButton.gameObject.activeSelf) SwapShopButton.gameObject.SetActive(true);

        if (BuyShopUI.activeSelf) BuyShopUI.SetActive(false);
        if (MainShopUI.activeSelf) MainShopUI.SetActive(false);

        SellShopUI.SetActive(true);
    }
    #endregion
}
