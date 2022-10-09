using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{
    public List<DialogueOption> greetingDialogue;

    public List<DialogueOption> goodbyeDialogue;

    public List<DialogueOption> changeTopicDialogue;

    [HideInInspector]public NPCDialogue questions;

    [Serializable]
    public struct PlayerQuestions
    {
        public NPCInfo npc;
        public List<DialogueOption> questionsForNPC;
    }

    public List<PlayerQuestions> playerQuestions;


    //Set the questions the player can ask the npc
    public void SetPlayerInquiriesForNPC(NPCInfo npc, NPCDialogue npcDialogue)
    {
        questions = new NPCDialogue();

        questions.dialogue = npcDialogue.dialogue;

        //enable the player to choose a dialogue option
        questions.requiresResponse = true;

        for (int i = 0; i < playerQuestions.Count; i++)
        {
            if (playerQuestions[i].npc == npc)
            {
                questions.playerResponses = playerQuestions[i].questionsForNPC;
            }
        }
    }
}
