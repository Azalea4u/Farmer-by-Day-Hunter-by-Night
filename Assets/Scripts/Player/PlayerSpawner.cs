using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject Player01;
    [SerializeField] private GameObject Player02;
    [SerializeField] private GameObject Player03;
    [SerializeField] private GameObject Player04;

    private NetworkObject netObj;

    private static PlayerSpawner instance;

    private PlayerSpawner() { }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        // Checks if current number of Players is over 4
        // If so, prevents other Players from joining the "world" / host Player's server

        if (NetworkManager.Singleton.ConnectedClients.Count <= 4) SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, NetworkManager.Singleton.ConnectedClients.Count);
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

        // Adds new Player to Host Player's world data
        Player p = newPlayer.GetComponentInChildren<Player>();
        p.playerData.currentScene = SceneManager.GetActiveScene().name;
        GameManager.instance.worldData.players.Add(p);
    }
}
