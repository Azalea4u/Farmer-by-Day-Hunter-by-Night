// Taken from https://www.youtube.com/watch?v=DXsmhMMH9h4
// Thank you very much for the help!

using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

// Handles Relay Server/Client Connections

public class RelayManager : MonoBehaviour
{
    [SerializeField] private RelayUI RUI;  // Used to get & display Join Code

    public static RelayManager instance;


    public async void StartRelay()
    {
        GameManager.instance.worldData.players.Clear();

        if (RUI.JoinCode == "") RUI.JoinCode = await StartHostWithRelay();
    }

    public async void JoinRelay()
    {
        if (RUI.JoinCode != "" && !NetworkManager.Singleton.IsConnectedClient)
        {
            try { await StartClientWithRelay(RUI.JoinCode); }
            catch { Debug.Log("Invalid join code"); }
        }
    }

    public void DisconnectRelay()
    {
        // Disconnects all other connected Players (if you are the host)
        if (NetworkManager.Singleton.IsHost)
        {
            for (int i = 1; i < NetworkManager.Singleton.ConnectedClients.Count; i++) { NetworkManager.Singleton.DisconnectClient((ulong)i); }
        }

        // Disconnects self
        NetworkManager.Singleton.Shutdown();
    }

    private async void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            await UnityServices.InitializeAsync();

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        else { Destroy(gameObject); }
    }

    private async Task<string> StartHostWithRelay(int maxConnections = 3)
    {
        Allocation allocation;
        string joinCode;

        try { allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections); }
        catch
        {
            Debug.LogError("Failed to create allocation");
            throw;
        }

        RelayServerData relayServerData = AllocationUtils.ToRelayServerData(allocation, "udp");  // Can change connectionType to dtls

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        try { joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId); }
        catch
        {
            Debug.LogError("Failed to get join code");
            throw;
        }

        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    private async Task<bool> StartClientWithRelay(string joinCode)
    {
        JoinAllocation joinAllocation;

        try { joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode); }
        catch
        {
            Debug.LogError("Failed to create join allocation");
            throw;
        }

        RelayServerData relayServerData = AllocationUtils.ToRelayServerData(joinAllocation, "udp");  // Can change connectionType to dtls

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }
}
