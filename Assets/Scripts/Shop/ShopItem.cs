using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ShopItem : Item
{
    private Button button;
    private Image sprite;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => ShopManager.instance.SelectItem(this));

        sprite = GetComponent<Image>();
        sprite.sprite = data.Icon;
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
