using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ShopItem : Item
{
    private Image sprite;

    private void Awake()
    {
        sprite = GetComponent<Image>();
        sprite.sprite = data.Icon;
    }


}
