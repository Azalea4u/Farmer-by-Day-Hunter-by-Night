using Unity.Netcode;
using UnityEngine;

public class ShopNPC : NetworkBehaviour, INPC
{
    public string Name { get; private set; }

    public void Talk()
    {
        print("hello, I am the shop");
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
