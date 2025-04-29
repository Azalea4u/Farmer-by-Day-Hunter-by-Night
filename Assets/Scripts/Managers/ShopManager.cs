using Unity.Netcode;
using UnityEngine;

public class ShopManager : NetworkBehaviour
{
    public static ShopManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
