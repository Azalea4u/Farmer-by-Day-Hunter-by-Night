using System.Collections.Generic;
using Unity.Netcode;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class ShopNPC : NetworkBehaviour, INPC
{
    public string Name { get; private set; }
    public Player CurrentTargetPlayer { get; private set; }


    public void TalkTo(Player targetPlayer)
    {
        print("Welcome to my shop!");
        CurrentTargetPlayer = targetPlayer;

        ShopManager.instance.OpenShop(CurrentTargetPlayer);
    }
}
