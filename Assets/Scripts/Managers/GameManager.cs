using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool IsGamePaused = false;

    [Header("Managers")]
    public ShopManager shopManager;
    public ItemManager itemManager;
    public DialogueManager dialogueManager;
    public UI_Manager uiManager;
    public TileManager tileManager;

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

        itemManager = GetComponent<ItemManager>();
        uiManager = GetComponent<UI_Manager>();
    }

    // Pauses the Game from any script
    public void PauseGame(bool pauseGame)
    {
        IsGamePaused = pauseGame;
        Time.timeScale = IsGamePaused ? 0 : 1;
        //Debug.Log("Game " + (IsGamePaused ? "Paused" : "Resumed"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
