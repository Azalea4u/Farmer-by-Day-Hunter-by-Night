using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
    [SerializeField] private GameObject Fade_UI;
    [SerializeField] private Image fadeImage; // Set this via inspector (a fullscreen black Image with 0 alpha)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Fade_UI.SetActive(false);
    }

    public void MainMenu()
    {
        AudioManager.instance.ChangeSceneWithMusic(Loader.scenes.SCN_MainMenu, "MainTheme_Music");
    }

    public void Load_VillageScene()
    {
        StartCoroutine(FadeAndLoadScene(Loader.scenes.ShopTest_Dialogue, "Village_Music", useFullLoading: false));
    }

    public void Load_FarmScene()
    {
        StartCoroutine(FadeAndLoadScene(Loader.scenes.SCN_FarmScene, "Farm_Music", useFullLoading: false));
    }

    public void Load_NorthForest()
    {
        StartCoroutine(FadeAndLoadScene(Loader.scenes.SCN_NorthForest, "NorthForest_Music", useFullLoading: false));
    }

    public void Load_SouthForest()
    {
        StartCoroutine(FadeAndLoadScene(Loader.scenes.SCN_SouthForest, "SouthForest_Music", useFullLoading: false));
    }

    public void Load_GameFromMainMenu()
    {
        AudioManager.instance.ChangeSceneWithMusic(Loader.scenes.ShopTest_Dialogue, "Farm_Music"); // or SCN_Loading + Farm_Music
    }

    public void Load_NextDay()
    {
        AudioManager.instance.ChangeSceneWithMusic(Loader.scenes.SCN_FarmScene, "Farm_Music"); // or use SCN_Loading too
    }

    private IEnumerator FadeAndLoadScene(Loader.scenes sceneToLoad, string musicName, bool useFullLoading)
    {
        //GameManager.instance.PauseGame(false);
        Fade_UI.SetActive(true);

        // Fade to black
        yield return StartCoroutine(Fade(1f));

        if (useFullLoading)
        {
            AudioManager.instance.ChangeSceneWithMusic(sceneToLoad, musicName);
            yield break;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad.ToString());
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        AudioManager.instance.PlayMusic(musicName);

        // Fade from black
        yield return StartCoroutine(Fade(0f));

        Fade_UI.SetActive(false);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float fadeDuration = 0.5f;
        float startAlpha = fadeImage.color.a;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }
}
