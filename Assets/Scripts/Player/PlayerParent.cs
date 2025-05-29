using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerParent : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        // DontDestroyOnLoad(gameObject);
        // NetworkManager.Singleton.SceneManager.OnUnload += Unload;

        if (!IsOwner)
        {
            enabled = false;
            return;
        }
    }
}
