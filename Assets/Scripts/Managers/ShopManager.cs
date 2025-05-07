using Ink.Parsed;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

// Handles all Shop background logic & manages/displays Shop UI

public class ShopManager : NetworkBehaviour
{
    public static ShopManager instance;

    [SerializeField] private List<ShopItem> Stock = new List<ShopItem>();
    [SerializeField] private ShopItem CurrentlySelectedItem = null;
    [SerializeField] private int count = 1;

    [Header("Shop UI Elements (Main Parent)")]
    [SerializeField] private GameObject ShopUI;

    [Header("Main Shop UI Elements")]
    [SerializeField] private GameObject MainShopUI;

    [Header("Buy Shop UI Elements")]
    [SerializeField] private GameObject BuyShopUI;
    [SerializeField] private TMP_Text CostDisplay;
    [SerializeField] private TMP_Text CountDisplay;
    [SerializeField] private TMP_Text GoldDisplay;
    [SerializeField] private Button ExitShopButton;
    [SerializeField] private Image DisplayedItem;

    [Header("Sell Shop UI Elements")]
    [SerializeField] private GameObject SellShopUI;

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
        }
    }

    private void SetupShops()
    {
        // Add items to shop stock here
    }
    #endregion


    #region Buy Shop
    public void Buy()
    {
        if (CurrentlySelectedItem != null && targetPlayer.playerData.gold > CurrentlySelectedItem.BuyPrice * count)
        {
            targetPlayer.playerData.gold -= CurrentlySelectedItem.BuyPrice * count;
            targetPlayer.inventoryManager.Add("Hotbar", CurrentlySelectedItem);

            GoldDisplay.text = "Total Gold: " + targetPlayer.playerData.gold.ToString();

            print("Item bought!");
        }
    }

    public void ChangeCount(int value)
    {
        if (CurrentlySelectedItem != null && (value > 0 || count != 1))
        {
            count += value;
            CountDisplay.text = count.ToString();
            GoldDisplay.text = "Total Gold: " + targetPlayer.playerData.gold.ToString();
        }
    }

    public void ResetBuyShopDisplay()
    {
        CurrentlySelectedItem = null;
        DisplayedItem.sprite = null;
        count = 1;
        CountDisplay.text = "";
        CostDisplay.text = "";
        GoldDisplay.text = "Total Gold: " + targetPlayer.playerData.gold.ToString();

        if (BuyShopUI.activeSelf) BuyShopUI.SetActive(false);
    }

    public void SelectItem(ShopItem item)
    {
        CurrentlySelectedItem = item;
        DisplayedItem.sprite = item.data.Icon;
        count = 1;
        CountDisplay.text = "1";
        CostDisplay.text = item.BuyPrice.ToString() + " G";
    }

    public void ShowBuyShop()
    {
        if (MainShopUI.activeSelf) MainShopUI.SetActive(false);
        if (SellShopUI.activeSelf) SellShopUI.SetActive(false);

        BuyShopUI.SetActive(true);
    }
    #endregion


    #region Sell Shop
    public void ResetSellShopDisplay()
    {
        if (SellShopUI.activeSelf) SellShopUI.SetActive(false);
    }

    public void Sell()
    {

    }

    public void ShowSellShop()
    {
        if (BuyShopUI.activeSelf) BuyShopUI.SetActive(false);
        if (MainShopUI.activeSelf) MainShopUI.SetActive(false);

        SellShopUI.SetActive(true);
    }
    #endregion
}
