using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject.TryGetComponent<Player>(out var player))
        {
            if (player.IsOwner)
            {
                // Play fade out here?

                print("Scene Transition!");
            }
        }
    }
}
