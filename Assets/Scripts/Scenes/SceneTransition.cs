using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string nextScene;
    [SerializeField] private Direction direction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject.TryGetComponent<Player>(out var player))
        {
            if (player.IsOwner)
            {
                player.lastTravelledDirection = direction;
                player.currentScene = nextScene;
                ScreenManager.instance.SwitchScene(nextScene);
            }
        }
    }
}
