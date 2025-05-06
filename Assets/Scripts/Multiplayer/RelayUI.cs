using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RelayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI joinCodeText;
    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private Image connectedIndicator;

    private readonly UnityEvent<ulong> OnClientConnected;
    private readonly UnityEvent<ulong> OnClientDisconnected;

    public string JoinCode { 
        get { return joinCode; }
        set { joinCode = value; joinCodeText.text = value; }
    }
    [SerializeField] private string joinCode = "";

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectedCallback;

        connectedIndicator.color = Color.red;
    }
    
    private void OnDestroy()
    {
        try
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientDisconnectedCallback;
        }
        catch { }
    }

    private void OnClientConnectedCallback(ulong clientID)
    {
        Debug.Log($"Client connected: {clientID}");
        OnClientConnected?.Invoke(clientID);

        connectedIndicator.color = Color.green;
    }

    private void OnClientDisconnectedCallback(ulong clientID)
    {
        Debug.Log($"Client disconnected: {clientID}");
        OnClientDisconnected?.Invoke(clientID);

        connectedIndicator.color = Color.red;
        JoinCode = "";
    }

    public void SetJoinCode() { if (JoinCode == "") JoinCode = joinCodeInputField.text; }
}
