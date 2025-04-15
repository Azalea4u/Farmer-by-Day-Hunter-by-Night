using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class LoadingText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI loadingText;
    private float timer = 1f;

    async void Update()
    {
        timer = -Time.deltaTime;
        if (loadingText.text == "Loading.")
        {
            await Task.Delay(1000);
            loadingText.text = "Loading..";
        }
        else if (loadingText.text == "Loading..")
        {
            await Task.Delay(1000);
            loadingText.text = "Loading...";
        }
        else if (loadingText.text == "Loading...")
        {
            await Task.Delay(1000);
            loadingText.text = "Loading.";
        }
        if (timer < 0f) timer = 1f;
    }
}
