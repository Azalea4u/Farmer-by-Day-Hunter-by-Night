using System.Collections;
using UnityEngine;

public class HoverPopUp : MonoBehaviour
{
    [SerializeField] private ShopItem parentItem;
    [SerializeField] private CanvasGroup trans;

    private float fadeDuration = 0.5f;


    private void Start()
    {
        if (trans == null) trans = GetComponent<CanvasGroup>();
        InstaHidePopUp();
    }

    public void InstaHidePopUp()
    {
        trans.alpha = 0f;
    }

    public void ShowPopUp()
    {
        trans.alpha = 1f;
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
