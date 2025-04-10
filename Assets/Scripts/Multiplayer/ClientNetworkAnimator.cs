using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkAnimator : NetworkAnimator
{
    // Guarantees server is Client Authoritative
    protected override bool OnIsServerAuthoritative() { return false; }
}
