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
        // We need some way to limit the amount of players that can join the world to 4
        // This if-statement will work as a temporary solution
        // NOTE: does not prevent join allocation/connection; player prefab will just not spawn

        if (NetworkManager.Singleton.ConnectedClients.Count < 5) SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, NetworkManager.Singleton.ConnectedClients.Count);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong clientID, int prefabID)
    {
        GameObject newPlayer;

        if      (prefabID == 2) newPlayer = (GameObject) Instantiate(Player02);
        else if (prefabID == 3) newPlayer = (GameObject) Instantiate(Player03);
        else if (prefabID == 4) newPlayer = (GameObject) Instantiate(Player04);
        else                    newPlayer = (GameObject) Instantiate(Player01);

        netObj = newPlayer.GetComponent<NetworkObject>();
        newPlayer.SetActive(true);
        netObj.SpawnAsPlayerObject(clientID, true);
    }
}
