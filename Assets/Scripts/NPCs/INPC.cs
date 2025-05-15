using System.Runtime.InteropServices;
using Unity.Netcode;
using UnityEngine;

public interface INPC
{
    string Name { get; }
    Player CurrentTargetPlayer { get; }
    DialogueTrigger NPCDialogueTrigger { get; }

    void TalkTo(Player targetPlayer);
}
