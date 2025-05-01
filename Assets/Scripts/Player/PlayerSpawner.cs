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
        if (IsServer) SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, 0);
        // else SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, 1);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong clientID, int prefabID)
    {
        GameObject newPlayer;

        if      (prefabID == 1) newPlayer = (GameObject) Instantiate(Player02);
        else if (prefabID == 2) newPlayer = (GameObject) Instantiate(Player03);
        else if (prefabID == 3) newPlayer = (GameObject) Instantiate(Player04);
        else                    newPlayer = (GameObject) Instantiate(Player01);

        netObj = newPlayer.GetComponent<NetworkObject>();
        newPlayer.SetActive(true);
        netObj.SpawnAsPlayerObject(clientID, true);
    }
}
