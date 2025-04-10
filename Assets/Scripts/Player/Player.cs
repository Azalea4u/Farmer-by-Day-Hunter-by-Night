using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }
    }
}
