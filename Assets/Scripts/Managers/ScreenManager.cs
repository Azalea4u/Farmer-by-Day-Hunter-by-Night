using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;

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
    }

    public void MainMenu()
    {
        AudioManager.instance.ChangeSceneWithMusic(Loader.scenes.SCN_MainMenu, "MainTheme_Music");
    }

    // OnClick methods for the MainMenu Scene
    // FROM ANOTHER PROJECT!!! Just use this as a basis
    #region MainMenu Button
    public void RocketClick()
    {
        GameManager.instance.PauseGame(false);
        AudioManager.instance.ChangeSceneWithMusic(Loader.scenes.SCN_SpaceshipScene, "Spaceship_Music");
    }

    public void ArcheologyClick()
    {
        GameManager.instance.PauseGame(false);
        AudioManager.instance.ChangeSceneWithMusic(Loader.scenes.SCN_ArcheologyMinigame, "Arch_Music");
    }

    public void DragonClick()
    {
        GameManager.instance.PauseGame(false);
        AudioManager.instance.ChangeSceneWithMusic(Loader.scenes.SCN_FindDragonLuigi, "Finding_Music");
    }

    public void OctopusClick()
    {
        GameManager.instance.PauseGame(false);
        AudioManager.instance.ChangeSceneWithMusic(Loader.scenes.SCN_OctopusShooter, "Octopus_Music");
    }
    #endregion
}
