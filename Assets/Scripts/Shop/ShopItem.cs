using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ShopItem : Item
{
    private Button button;

    public TMP_Text quantityDisplay;
    public Image image;

    public bool canBuyItem = false;


    private void Awake()
    {
        button = GetComponent<Button>();
        if (canBuyItem) button.onClick.AddListener(() => ShopManager.instance.SelectItemToBuy(this));
        else button.onClick.AddListener(() => ShopManager.instance.SelectItemToSell(this));

        image = GetComponent<Image>();
        if (data != null) image.sprite = data.Icon;
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
