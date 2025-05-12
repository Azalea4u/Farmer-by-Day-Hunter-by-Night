using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ShopItem : Item, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hoverPopUp;

    private Button button;

    public TMP_Text quantityDisplay;
    public Image image;

    public bool canBuyItem = false;
    public int quantity = 1;

    public bool mouse_over = false;


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

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.SetAsLastSibling();

        mouse_over = true;
        hoverPopUp.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        hoverPopUp.SetActive(false);
    }
}
