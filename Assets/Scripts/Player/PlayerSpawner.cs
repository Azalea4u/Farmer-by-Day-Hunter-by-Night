using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject Player01;
    [SerializeField] private GameObject Player02;
    [SerializeField] private GameObject Player03;
    [SerializeField] private GameObject Player04;

    private NetworkObject netObj;


    public override void OnNetworkSpawn()
    {
        // Checks if current number of Players is over 4
        // If so, prevents other Players from joining the "world" / host Player's server

        if (NetworkManager.Singleton.ConnectedClients.Count < 5) SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, NetworkManager.Singleton.ConnectedClients.Count);
        else NetworkManager.Singleton.Shutdown();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong clientID, int prefabID)
    {
        GameObject newPlayer;

        if      (prefabID == 2) newPlayer = Instantiate(Player02);
        else if (prefabID == 3) newPlayer = Instantiate(Player03);
        else if (prefabID == 4) newPlayer = Instantiate(Player04);
        else                    newPlayer = Instantiate(Player01);

        netObj = newPlayer.GetComponent<NetworkObject>();
        newPlayer.SetActive(true);
        netObj.SpawnAsPlayerObject(clientID, true);
    }
}
