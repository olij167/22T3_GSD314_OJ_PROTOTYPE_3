using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "DialogueSystem/PlayerDialogueOption")]
public class DialogueOption : ScriptableObject
{
    [TextArea(3, 10)]
    public string dialogue;

    public NPCDialogue npcResponse;

    public List<NPCInfo.Emotion> emotionEffectsList;

    public void AdjustAttitudeValues(NPCInfo npc)
    {
        //for (int i = 0; i < emotionEffectsList.Count; i++)
        //{
        //    for (int n = 0; n < npc.emotionsList.Count; n++)
        //    {
        //        if (emotionEffectsList[i].mood == npc.emotionsList[n].mood)
        //        {
        //            npc.emotionsList[n].emotionValue += emotionEffectsList[i].emotionValue;
        //        }
        //    }
        //}
    }
}
