using UnityEngine;
using Unity.Netcode.Components;

public class ClientNetworkTransform : NetworkTransform
{
    // Guarantees server is Client Authoritative
    protected override bool OnIsServerAuthoritative() { return false; }
}
