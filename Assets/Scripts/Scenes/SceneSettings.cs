using UnityEngine;

public class SceneSettings : MonoBehaviour
{
    private bool settingsApplied = false;

    [Header("Scene Camera Bounds/Constraints")]
    public Vector2 CameraConstraint = new(20, 20);

    [Header("Is Forest Area?")]
    public bool isForestArea = false;

    [Header("Is Farm Area?")]
    public bool isFarmArea = false;

    [Header("Spawn Points")]
    public Vector2 northSpawn;
    public Vector2 eastSpawn;
    public Vector2 southSpawn;
    public Vector2 westSpawn;

    [Header("Forest Spawn Points")]
    public Vector2 villageSpawn;
    public Vector2 farmSpawn;


    private void Update()
    {
        if (!settingsApplied && GameManager.instance.player != null)
        {
            Player player = GameManager.instance.player;

            // This code is pretty "rigid" & a little clunky right now (my apologies) but it works!
            // If we come up with something better in the future, please replace this lol

            if (isForestArea)
            {
                switch (player.lastTravelledDirection)
                {
                    case Direction.NORTH:
                        {
                            if (player.inFarm) player.transform.position = farmSpawn;
                            else player.transform.position = villageSpawn;

                            break;
                        }
                    case Direction.EAST: player.transform.position = eastSpawn; break;
                    case Direction.SOUTH:
                        {
                            if (player.inFarm) player.transform.position = farmSpawn;
                            else player.transform.position = villageSpawn;

                            break;
                        }
                    case Direction.WEST: player.transform.position = westSpawn; break;
                }
            }
            else
            {
                switch (player.lastTravelledDirection)
                {
                    case Direction.NORTH: player.transform.position = southSpawn; break;
                    case Direction.EAST: player.transform.position = westSpawn; break;
                    case Direction.SOUTH: player.transform.position = northSpawn; break;
                    case Direction.WEST: player.transform.position = eastSpawn; break;
                }
            }

            if (player.playerCamera)
            {
                player.playerCamera.constraintX = CameraConstraint.x;
                player.playerCamera.constraintY = CameraConstraint.y;
            }

            player.inFarm = isFarmArea;
            
            settingsApplied = true;
        }
    }
}
