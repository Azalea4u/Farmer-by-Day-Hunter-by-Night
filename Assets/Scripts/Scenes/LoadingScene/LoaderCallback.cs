using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool IsFirstUpdate = true;

    private void Update()
    {
        if (IsFirstUpdate)
        {
            IsFirstUpdate = false;

            if (AudioManager.instance != null)
            {
                AudioManager.instance.OnLoadingScreenShown();
            }
        }
    }
}
