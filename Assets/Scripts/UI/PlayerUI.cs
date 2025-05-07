using Unity.Netcode;
using UnityEngine;

public class PlayerUI : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            gameObject.SetActive(false);
        }
    }
}
