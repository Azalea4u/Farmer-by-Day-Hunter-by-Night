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
    [SerializeField] private int count = 0;
    

    [Header("Buy Shop UI Elements")]
    [SerializeField] private GameObject BuyShopUI;
    [SerializeField] private TMP_Text CostDisplay;
    [SerializeField] private TMP_Text CountDisplay;
    [SerializeField] private Button ExitShopButton;
    [SerializeField] private Image DisplayedItem;

    // Set to public if in Player_UI scene and attach player object for testing
    private Player targetPlayer;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }

        SetupShop();

        BuyShopUI.SetActive(false);
    }

    private void SetupShop()
    {
        // Add items to shop stock here
    }

    public void Buy()
    {
        if (CurrentlySelectedItem != null && count != 0)
        {
            // No way to access any kind of PlayerData at the moment
            // Implementation might look something like this:
            // --> targetPlayer.data.gold -= item.BuyPrice;
            Debug.Log("Player's Gold: " + targetPlayer.playerData.gold);
            targetPlayer.playerData.gold -= CurrentlySelectedItem.BuyPrice;
            targetPlayer.inventoryManager.Add("Hotbar", CurrentlySelectedItem);
            
            print("Item bought!");
        }
    }

    public void Sell()
    {

    }

    public void OpenShop(Player player)
    {
        if (player.TryGetComponent<PlayerController>(out var controller))
        {
            ResetShopDisplay();
            BuyShopUI.SetActive(true);

            ExitShopButton.onClick.AddListener(() => controller.StopDialogue());

            targetPlayer = player;
        }
    }

    public void ExitShop()
    {
        if (targetPlayer.TryGetComponent<PlayerController>(out var controller))
        {
            BuyShopUI.SetActive(false);

            // I am not entirely sold on this approach (adding/removing listeners)
            // But it seems to work nicely, even in multiplayer, so I'll take it!

            ExitShopButton.onClick.RemoveListener(() => controller.StopDialogue());

            targetPlayer = null;
        }
    }

    public void ResetShopDisplay()
    {
        CurrentlySelectedItem = null;
        DisplayedItem.sprite = null;
        count = 0;
        CountDisplay.text = "";
        CostDisplay.text = "";
    }

    public void SelectItem(ShopItem item)
    {
        CurrentlySelectedItem = item;
        DisplayedItem.sprite = item.data.Icon;
        count = 0;
        CountDisplay.text = "0";
        CostDisplay.text = item.BuyPrice.ToString() + " G";
    }

    public void ChangeCount(int value) 
    {
        if (CurrentlySelectedItem != null && (value > 0 || count != 0))
        {
            count += value;
            CountDisplay.text = count.ToString();
        }
    }
}
