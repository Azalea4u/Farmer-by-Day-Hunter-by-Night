using System.Collections;
using TMPro;
using UnityEngine;

public class HoverPopUp : MonoBehaviour
{
    [SerializeField] private ShopItem parentItem;
    [SerializeField] private CanvasGroup trans;

    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text buyPrice;
    [SerializeField] private TMP_Text sellPrice;

    private readonly float fadeDuration = 0.5f;


    private void Start()
    {
        if (trans == null) trans = GetComponent<CanvasGroup>();
        InstaHidePopUp();
    }

    public void InstaHidePopUp()
    {
        trans.alpha = 0f;

        if (buyPrice != null && buyPrice.gameObject.activeSelf) buyPrice.gameObject.SetActive(false);
        if (sellPrice != null && sellPrice.gameObject.activeSelf) sellPrice.gameObject.SetActive(false);
    }

    public void ShowPopUp()
    {
        trans.alpha = 1f;

        if (title != null) title.text = parentItem.ItemName;
        if (description != null) description.text = parentItem.Description;

        if (parentItem.canBuyItem == true && buyPrice != null)
        {
            buyPrice.text = "Cost: " + parentItem.BuyPrice + " G";
            buyPrice.gameObject.SetActive(true);
        }

        if (parentItem.canBuyItem == false && sellPrice != null)
        {
            sellPrice.text = "Sell: " + parentItem.SellPrice + " G";
            sellPrice.gameObject.SetActive(true);
        }
    }

    public void HidePopUp()
    {
        StartCoroutine(Fade(trans.alpha, 0f));
    }

    // Didn't really like the look of the constant fades, but might be worth coming back to / refining this later
    private IEnumerator Fade(float start, float end)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            trans.alpha = Mathf.Lerp(start, end, elapsedTime / fadeDuration);
            yield return null;
        }

        trans.alpha = end;
    }
}
