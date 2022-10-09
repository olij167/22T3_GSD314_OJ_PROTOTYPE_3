using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("DialogueSystem/NPCInfo"))]
public class NPCInfo : ScriptableObject
{
    public string npcName;

    public List<NPCDialogue> greetingDialogue;

    public List<NPCDialogue> goodbyeDialogue;

    public List<NPCDialogue> changeTopicDialogue;

    [System.Serializable]
    public enum Mood { calm, happy, sad, angry, nervous, scared, surprised }

    [System.Serializable]
    public struct Emotion { public Mood mood; public float emotionValue; }

    public List<Emotion> emotionsList;





    //public float GetHighestAttitudeValue()
    //{
    //    List<float> attitudeValues = new List<float>(7);
    //    attitudeValues[0] = calm;
    //    attitudeValues[1] = happy;
    //    attitudeValues[2] = sad;
    //    attitudeValues[3] = angry;
    //    attitudeValues[4] = nervous;
    //    attitudeValues[5] = scared;
    //    attitudeValues[6] = surprised;

    //    for (int i = 0; i < attitudeValues.Count; i++)
    //    {
    //        if (i < 7)
    //        {
    //            if (attitudeValues[i] > attitudeValues[i + 1])
    //            {
    //                attitudeValues.Remove(i + 1);
    //            }
    //            else attitudeValues.Remove(i);
    //        }
    //    }

    //    return attitudeValues[0];
    //}


}



//create (structs?) t 