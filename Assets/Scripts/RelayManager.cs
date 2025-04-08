// Taken from https://www.youtube.com/watch?v=DXsmhMMH9h4
// Thank you very much for the help!

using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI joinCodeText;
    [SerializeField] private TMP_InputField joinCodeInputField;


    public async void StartRelay()
    {
        string joinCode = await StartHostWithRelay();
        joinCodeText.text = joinCode;
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private async Task<string> StartHostWithRelay(int maxConnections = 3)
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);

        RelayServerData relayServerData = AllocationUtils.ToRelayServerData(allocation, "udp");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    private async Task<bool> StartClientWithRelay(string joinCode)
    {
        return false;
    }
}
