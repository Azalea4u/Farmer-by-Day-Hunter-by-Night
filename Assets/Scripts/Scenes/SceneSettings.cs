using UnityEngine;

public class SceneSettings : MonoBehaviour
{
    public Vector2 CameraConstraint = new(20, 20);

    private bool settingsApplied = false;


    private void Update()
    {
        if (!settingsApplied && GameManager.instance.player != null)
        {
            Player player = GameManager.instance.player;

            if (player.playerCamera)
            {
                player.playerCamera.constraintX = CameraConstraint.x;
                player.playerCamera.constraintY = CameraConstraint.y;
            }
            
            settingsApplied = true;
        }
    }
}
