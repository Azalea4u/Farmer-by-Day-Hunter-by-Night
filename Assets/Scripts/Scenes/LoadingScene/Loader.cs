using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private static scenes TargetScene;

    public enum scenes
    {
        // ACTUAL GAME SCENES
        SCN_Loading,
        SCN_MainMenu,
        SCN_VillageScene,
        SCN_FarmScene,
        SCN_NorthForest,
        SCN_SouthForest,
        
        // TEST SCENES
        Player_UI,
        ShopTest_Dialogue
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
