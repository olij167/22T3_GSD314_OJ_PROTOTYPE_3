using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    public DialogueSystem dialogueSystem;
    [SerializeField] private PlayerDialogue playerDialogue;

    private PatrolToRandomWayPoint npcPatrol;


    public void EnterDialogue(NPCInfo npc)
    {
        //Set NPC
        //Set greeting text
        //Set player options
        dialogueSystem.enabled = true;

        dialogueSystem.npc = npc;
        dialogueSystem.npc.npcEmotions.SetMood();
        dialogueSystem.npcNameText.text = npc.npcName;

        NPCDialogueOption greetingDialogue = npc.npcDialogue.greetingDialogue[Random.Range(0, npc.npcDialogue.greetingDialogue.Count)];


        greetingDialogue.playerResponses = playerDialogue.SetPlayerQuestionsForNPC(npc, greetingDialogue).playerResponses;


        dialogueSystem.playerDialogueText.text = playerDialogue.greetingDialogue[Random.Range(0, playerDialogue.greetingDialogue.Count)].dialogue;

        

        //Deactivate Player Controller
        dialogueSystem.playerMovement.enabled = false;
        dialogueSystem.playerCam.enabled = false;

        //Lock Camera to NPC target


        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = true;
        dialogueSystem.BeginDialogue();

        dialogueSystem.SetNewDialogueText(greetingDialogue);

    }

    public void NPCInitiatedDialogue(NPCInfo npc, NPCDialogueOption startingDialogue)
    {
        //Set NPC
        //Set greeting text
        //Set player options
        dialogueSystem.enabled = true;

        dialogueSystem.npc = npc;
        dialogueSystem.npc.npcEmotions.SetMood();
        dialogueSystem.npcNameText.text = npc.npcName;

        //NPCDialogueOption greetingDialogue = npc.npcDialogue.greetingDialogue[Random.Range(0, npc.npcDialogue.greetingDialogue.Count)];


        //greetingDialogue.playerResponses = playerDialogue.SetPlayerQuestionsForNPC(npc, greetingDialogue).playerResponses;


        dialogueSystem.playerDialogueText.text = playerDialogue.greetingDialogue[Random.Range(0, playerDialogue.greetingDialogue.Count)].dialogue;



        //Deactivate Player Controller
        dialogueSystem.playerMovement.enabled = false;
        dialogueSystem.playerCam.enabled = false;

        //Lock Camera to NPC target


        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = true;
        dialogueSystem.BeginDialogue();

        dialogueSystem.SetNewDialogueText(startingDialogue);
    }

    public void EnterDialogueWithRandomNPC()
    {
        EnterDialogue(GetRandomNPC());
    }

    public NPCInfo GetRandomNPC()
    {
        int rand = Random.Range(0, playerDialogue.playerQuestions.Count);

        NPCInfo npc = playerDialogue.playerQuestions[rand].npc;

        return npc;
    }
}
