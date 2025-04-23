using System.Runtime.InteropServices;
using Unity.Netcode;
using UnityEngine;

public interface INPC
{
    string Name { get; }
    Player CurrentTargetPlayer { get; }

    void Talk(Player targetPlayer);
}
