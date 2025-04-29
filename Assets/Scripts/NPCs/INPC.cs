using System.Runtime.InteropServices;
using Unity.Netcode;
using UnityEngine;

public interface INPC
{
    string Name { get; }
    PlayerMovement CurrentTargetPlayer { get; }

    void Talk(PlayerMovement targetPlayer);
}
