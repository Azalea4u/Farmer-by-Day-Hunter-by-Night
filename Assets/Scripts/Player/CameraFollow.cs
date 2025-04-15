using UnityEngine;
using Unity.Netcode;

// https://discussions.unity.com/t/restricting-camera-to-within-areas-2d-adventure-platformer/107389, this is quite clever, thank you!

public class CameraFollow : NetworkBehaviour
{
    public float constraintX = 100;
    public float constraintY = 100;

    [SerializeField] private Transform player;

    private void Update()
    {
        Vector3 position = transform.position;

        if (Mathf.Abs(player.position.x) < constraintX) position.x = player.position.x;
        if (Mathf.Abs(player.position.y) < constraintY) position.y = player.position.y;

        transform.position = position;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            gameObject.SetActive(false);
        }
    }
}
