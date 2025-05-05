using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool IsGamePaused = false;

    [Header("Managers")]
    public ShopManager shopManager;
    public ItemManager itemManager;
    public DialogueManager dialogueManager;

    public Player player;


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

    // Pauses the Game from any script
    public void PauseGame(bool pauseGame)
    {
        IsGamePaused = pauseGame;
        Time.timeScale = IsGamePaused ? 0 : 1;
        //Debug.Log("Game " + (IsGamePaused ? "Paused" : "Resumed"));
    }

}
