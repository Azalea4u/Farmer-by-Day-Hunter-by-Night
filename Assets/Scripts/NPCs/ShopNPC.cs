using System.Collections.Generic;
using Unity.Netcode;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class ShopNPC : NetworkBehaviour, INPC
{
    [SerializeField] private GameObject ShopUI;
    [SerializeField] private Button ShopCloseButton;
    [SerializeField] private Image DisplayedItem;
    
    private List<ShopItem> Stock = new List<ShopItem>();

    public string Name { get; private set; }
    public Player CurrentTargetPlayer { get; private set; }

    public ShopItem SelectedItem = null;


    private void Awake()
    {
        ShopUI.SetActive(false);
    }

    private void SetupShop()
    {
        // Add items to shop stock here
    }

    public void SelectItem(ShopItem item)
    {
        SelectedItem = item;
        DisplayedItem.sprite = item.data.Icon;
    }

    public void Buy()
    {

    }

    public void Sell()
    {

    }

    public void Talk(Player targetPlayer)
    {
        print("Welcome to my shop!");
        CurrentTargetPlayer = targetPlayer;

        ShopUI.SetActive(true);
        ShopCloseButton.onClick.AddListener(() => CurrentTargetPlayer.StopDialogue());
    }

    public void ExitShop()
    {
        ShopUI.SetActive(false);

        // I am not entirely sold on this approach (adding/removing listeners)
        // But it seems to work nicely, even in multiplayer, so I'll take it!

        ShopCloseButton.onClick.RemoveListener(() => CurrentTargetPlayer.StopDialogue());
    }
}
