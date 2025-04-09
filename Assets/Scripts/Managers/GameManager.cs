using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isGamePaused = false;

    private void Awake()
    {
        // Singleton pattern for GameManager
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject);

    }

}
