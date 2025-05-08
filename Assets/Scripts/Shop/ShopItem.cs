using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ShopItem : Item
{
    private Button button;
    private Image sprite;

    public bool canBuyItem = false;


    private void Awake()
    {
        button = GetComponent<Button>();
        if (canBuyItem) button.onClick.AddListener(() => ShopManager.instance.SelectItemToBuy(this));
        else button.onClick.AddListener(() => ShopManager.instance.SelectItemToSell(this));

        sprite = GetComponent<Image>();
        sprite.sprite = data.Icon;
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
