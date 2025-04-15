using Unity.Netcode;
using UnityEngine;

public class PlayerParent : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }
    }
}
