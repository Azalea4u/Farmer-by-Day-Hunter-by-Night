using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ShopNPC : NetworkBehaviour, INPC
{
    [SerializeField] private GameObject ShopUI;
    [SerializeField] private Button ShopCloseButton;

    public string Name { get; private set; }
    public Player CurrentTargetPlayer { get; private set; }

    private void Awake()
    {
        ShopUI.SetActive(false);
    }

    public void Talk(Player targetPlayer)
    {
        print("hello, I am the shop");
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
