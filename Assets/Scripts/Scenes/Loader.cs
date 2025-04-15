using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private static scenes TargetScene;

    public enum scenes
    {
        // Add scenes by scene name such as:
        SCN_MainMenu,
        SCN_Loading,
        SCN_ArcheologyMinigame,
        SCN_SpaceshipScene,
        SCN_FindDragonLuigi,
        SCN_OctopusShooter
    }

    public static void Load(scenes targetScene)
    {
        Loader.TargetScene = targetScene;

        SceneManager.LoadScene(Loader.scenes.SCN_Loading.ToString());
    }

    public static void ContinueToTargetScene()
    {
        SceneManager.LoadScene(TargetScene.ToString());

        if (AudioManager.instance != null)
        {
            AudioManager.instance.OnSceneLoaded();
        }
    }
}
