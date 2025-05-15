using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(DialogueTrigger))]
public class ShopNPC : NetworkBehaviour, INPC
{
    public string Name { get; private set; }
    public Player CurrentTargetPlayer { get; private set; }
    public DialogueTrigger NPCDialogueTrigger { get; private set; }


    private void Start() { NPCDialogueTrigger = GetComponent<DialogueTrigger>(); }

    public void TalkTo(Player targetPlayer)
    {
        if (!DialogueManager.GetInstance().dialogueIsPlaying)
        {
            print("Welcome to my shop!");
            CurrentTargetPlayer = targetPlayer;

            ShopManager.instance.OpenShop(CurrentTargetPlayer);

            DialogueManager.GetInstance().EnterDialogueMode(NPCDialogueTrigger.GetInk());
        }
    }
}
