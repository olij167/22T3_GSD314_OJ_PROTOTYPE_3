using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    [SerializeField] private DialogueSystem dialogueSystem;
    [SerializeField] private PlayerDialogue playerDialogue;
    public void EnterDialogue(NPCInfo npc)
    {
        //Set NPC
        //Set greeting text
        //Set player options

        dialogueSystem.npc = npc;
        NPCDialogue greetingDialogue = npc.greetingDialogue[Random.Range(0, npc.greetingDialogue.Count)];
        playerDialogue.SetPlayerInquiriesForNPC(npc, greetingDialogue);

        //Deactivate Player Controller
        //Lock Camera to NPC target

        dialogueSystem.enabled = true;
        dialogueSystem.BeginDialogue();
    }
}
