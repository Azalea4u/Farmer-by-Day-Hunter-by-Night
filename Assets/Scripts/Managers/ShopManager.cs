using Ink.Parsed;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

// Handles all Shop background logic & manages/displays Shop UI

public class ShopManager : NetworkBehaviour
{
    public static ShopManager instance;

    [SerializeField] private List<ShopItem> Stock = new List<ShopItem>();
    [SerializeField] private ShopItem CurrentlySelectedItem = null;

    [Header("UI Elements")]
    [SerializeField] private GameObject ShopUI;
    [SerializeField] private Button ExitShopButton;
    [SerializeField] private Image DisplayedItem;

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

        ShopUI.SetActive(false);
    }

    private void SetupShop()
    {
        // Add items to shop stock here
    }

    public void Buy()
    {
        if (CurrentlySelectedItem != null) { print("Item bought!"); }
    }

    public void Sell()
    {

    }

    public void OpenShop(Player player)
    {
        if (player.TryGetComponent<PlayerController>(out var controller))
        {
            ResetShopDisplay();
            ShopUI.SetActive(true);

            ExitShopButton.onClick.AddListener(() => controller.StopDialogue());

            targetPlayer = player;
        }
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

    public void ResetShopDisplay()
    {
        CurrentlySelectedItem = null;
        DisplayedItem.sprite = null;
    }

    public void SelectItem(ShopItem item)
    {
        CurrentlySelectedItem = item;
        DisplayedItem.sprite = item.data.Icon;
    }
}
