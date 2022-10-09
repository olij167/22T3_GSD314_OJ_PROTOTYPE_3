using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private bool inDialogue;
    [SerializeField] private bool playerIsSpeaking;

    public NPCInfo npc;

    //Dialogue to display in npc text box
    [SerializeField] private NPCDialogue otherDialogue;

    //Dialogue to display in the player text box
    [SerializeField] private DialogueOption selectedDialogueOption;

    //Dialogue UI
    [SerializeField] private TextMeshProUGUI otherDialogueText;
    [SerializeField] private TextMeshProUGUI playerDialogueText;

    //player Dialogue Selection UI
    [SerializeField] private List<Image> selectedOptionImages;
    [SerializeField] private TextMeshProUGUI selectedOptionText;

    [SerializeField] private bool playerResponseLockedIn;

    //Response Timer Variables
    private bool responseTimerActive;
    private float responseTimer = 5f;
    private float responseTimerReset;

    [SerializeField] private Slider responseTimerUI;
    [SerializeField] private bool timerAutoStart; 
        // if true the timer will automatically start during a time-limited response and pick a random option if the player doesn't begin viewing the dialogue options
        // if false the timer won't start until the player has begun viewing the dialogue options

    //Default player dialogue
    [SerializeField] private PlayerDialogue playerDialogue;

    [SerializeField] private GameObject dialogueUI;

    private int WElementNum = 0, AElementNum = 1, DElementNum = 2, SElementNum = 3;



    private void Start()
    {
        SetNewDialogueText();
        SetResponseTimer();
        playerDialogueText.text = " ";
    }

    private void Update()
    {
        if (inDialogue)
        {
            if (!playerResponseLockedIn)
            {
                if (otherDialogue.requiresResponse)
                {
                    ViewFullDialogueOption();
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        LockInResponse();
                    }
                }

                if (responseTimerActive)
                {
                    if (timerAutoStart) //start the timer straight away
                    {
                        responseTimer -= Time.deltaTime;
                        responseTimerUI.value = responseTimer;

                        if (responseTimer <= 0f)
                        {
                            // select a random option if the player doesn't have anything selected
                            if (selectedDialogueOption == null)
                            {
                                selectedDialogueOption = otherDialogue.playerResponses[Random.Range(0, otherDialogue.playerResponses.Count)];
                                playerDialogueText.text = selectedDialogueOption.dialogue;
                            }

                            LockInResponse();
                        }
                    }
                    else if (selectedDialogueOption != null) // start the timer after the player has selected an option
                    {
                        responseTimer -= Time.deltaTime;
                        responseTimerUI.value = responseTimer;

                        if (responseTimer <= 0f)
                        {
                            LockInResponse();
                        }
                    }
                }

                //Manually lock in player Answer
                if (selectedDialogueOption != null)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        LockInResponse();
                    }
                }
            }
            else if (selectedOptionText.text != "Q")
            {
                //Progress Conversation

                if (selectedOptionText.text != "E")
                {
                    // Check if dialogue requires the player to continue the conversation
                    if (!otherDialogue.requiresResponse)
                    {
                        //If it doesn't the npc dialogue will only read the first element from the current npc DialogueOption's response list
                        otherDialogue = otherDialogue.continuedDialogue;
                    }
                    else
                    {
                        // if the player responds the npc dialogue will only read the first element from the selected DialogueOption's response list
                        otherDialogue = selectedDialogueOption.npcResponse;
                    }
                }

                SetNewDialogueText();
                
            }
        }
    }

    private void ViewFullDialogueOption()
    {
        //select W response
        if (otherDialogue.playerResponses.Count >= 1)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                playerDialogueText.text = otherDialogue.playerResponses[WElementNum].dialogue;
                selectedDialogueOption = otherDialogue.playerResponses[WElementNum];
                HighlightSelectedOption(0);
                selectedOptionText.text = "W";
                Debug.Log("Displaying W Option");
            }
        }

        //select A response
        if (otherDialogue.playerResponses.Count >= 2)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                playerDialogueText.text = otherDialogue.playerResponses[AElementNum].dialogue;
                selectedDialogueOption = otherDialogue.playerResponses[AElementNum];
                HighlightSelectedOption(1);
                selectedOptionText.text = "A";
                Debug.Log("Displaying A Option");

            }
        }

        //select D response
        if (otherDialogue.playerResponses.Count >= 3)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                playerDialogueText.text = otherDialogue.playerResponses[DElementNum].dialogue;
                selectedDialogueOption = otherDialogue.playerResponses[DElementNum];
                HighlightSelectedOption(2);
                selectedOptionText.text = "D";
                Debug.Log("Displaying D Option");


            }
        }

        //select S response
        if (otherDialogue.playerResponses.Count >= 4)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                playerDialogueText.text = otherDialogue.playerResponses[SElementNum].dialogue;
                selectedDialogueOption = otherDialogue.playerResponses[SElementNum];
                HighlightSelectedOption(3);
                selectedOptionText.text = "S";
                Debug.Log("Displaying S Option");


            }
        }

        //select Leave Conversation
        if (playerIsSpeaking && otherDialogue.canChangeTopic && !otherDialogue.limitedTime)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                int rand = Random.Range(0, playerDialogue.goodbyeDialogue.Count);
                playerDialogueText.text = playerDialogue.goodbyeDialogue[rand].dialogue;
                selectedDialogueOption = playerDialogue.goodbyeDialogue[rand];
                HighlightSelectedOption(4);
                selectedOptionText.text = "Q";
                Debug.Log("Displaying Q Option");
            }
        }

        //select Change Topic / View more responses
        if (playerIsSpeaking && otherDialogue.canChangeTopic && !otherDialogue.limitedTime)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                int rand = Random.Range(0, playerDialogue.changeTopicDialogue.Count);
                playerDialogueText.text = playerDialogue.changeTopicDialogue[rand].dialogue;
                selectedDialogueOption = playerDialogue.changeTopicDialogue[rand];
                HighlightSelectedOption(5);
                selectedOptionText.text = "E";
                Debug.Log("Displaying E Option");
            }
        }

    }

    private void SetNewDialogueText()
    {
        otherDialogueText.text = otherDialogue.dialogue;

        playerResponseLockedIn = false;
        ResetHighlightedOption();

        if (otherDialogue.requiresResponse)
        {
            if(!otherDialogue == playerDialogue.questions)
            {
                playerIsSpeaking = false;
            }

            CheckValidDialougeOptions();
        }
        else
        {
            foreach(Image image in selectedOptionImages)
            {
                image.color = Color.grey;
            }
        }

        SetResponseTimer();

    }

    private void CheckValidDialougeOptions()
    {
        switch(otherDialogue.playerResponses.Count)
        {
            case 0:
                BlockDialogueOption(0);
                BlockDialogueOption(1);
                BlockDialogueOption(2);
                BlockDialogueOption(3);
                break;
            case 1:
                BlockDialogueOption(1);
                BlockDialogueOption(2);
                BlockDialogueOption(3);
                break;
            case 2:
                BlockDialogueOption(2);
                BlockDialogueOption(3);
                break;
            case 3:
                BlockDialogueOption(3);
                break;
            default:
                break;
        }
    }

    //Change unselectable options to grey
    private void BlockDialogueOption(int i)
    {
        selectedOptionImages[i].color = Color.grey;
    }

  

    //set selected option to yellow
    private void HighlightSelectedOption(int i)
    {
        selectedOptionImages[i].color = Color.yellow;

        foreach (Image optionUI in selectedOptionImages)
        {
            if (optionUI != selectedOptionImages[i] && optionUI.color != Color.grey)
            {
                optionUI.color = Color.white;
            }
        }
    }

    //Set all options to white
    private void ResetHighlightedOption()
    {
        selectedDialogueOption = null;
        selectedOptionText.text = "?";

        foreach (Image optionUI in selectedOptionImages)
        {
            optionUI.color = Color.white;
        }
    }

    private void LockInResponse()
    {
        playerResponseLockedIn = true;
        responseTimer = responseTimerReset;
        responseTimerUI.value = responseTimer;
        responseTimerActive = false;

        selectedDialogueOption.AdjustAttitudeValues(npc);

        if (selectedOptionText.text == "E") // TO DO: grey out 'E' ui if the player isnt responding & doesnt have surplus dialog topics
        {
            if (!playerIsSpeaking)
            {
                ChangeTopic();
            }
            else if (playerDialogue.questions.playerResponses.Count > 4)
            {
                ViewOtherTopics();
            }
            //else BlockDialogueOption(5);
        }

        if (selectedOptionText.text == "Q")
        {
            LeaveDialogue();
        }
    }

    private void SetResponseTimer()
    {
        if (otherDialogue.limitedTime)
        {
            responseTimer = otherDialogue.timeLimit;

            responseTimerReset = responseTimer;
            responseTimerUI.maxValue = responseTimerReset;
            responseTimerUI.value = responseTimer;

            responseTimerActive = true;

        }
        else
        {
            responseTimerReset = responseTimer;
            responseTimerUI.maxValue = responseTimerReset;
            responseTimerUI.value = responseTimer;

            responseTimerActive = false;

        }
    }

    public void BeginDialogue()
    {
        inDialogue = true;
        dialogueUI.SetActive(true);
        playerIsSpeaking = true;

    }

    private void LeaveDialogue()
    {
        dialogueUI.SetActive(false);
        inDialogue = false;

        //Activate Player Controller
        //Unlock Camera
    }

    private void ChangeTopic()
    {
        // get stored inquiries depending on NPC
        playerDialogue.SetPlayerInquiriesForNPC(npc, npc.changeTopicDialogue[Random.Range(0, npc.changeTopicDialogue.Count)]);
        otherDialogue = playerDialogue.questions;
        playerDialogue.questions.dialogue = otherDialogue.dialogue;

        playerIsSpeaking = true;

        Debug.Log("Changing the topic");
    }

    // scroll up and down through dialogue options
    private void ViewOtherTopics()
    {
        int numOfInquiries = playerDialogue.questions.playerResponses.Count - 1;
       
        if (WElementNum + 4 <= numOfInquiries)
        {
            WElementNum += 4;
        }
        else if (WElementNum - 4 >= 0)
        {
            WElementNum -= 4;
        }

        if (AElementNum + 4 <= numOfInquiries)
        {
            AElementNum += 4;
        }
        else if (AElementNum - 4 >= 1)
        {
            AElementNum -= 4;
        }

        if (DElementNum + 4 <= numOfInquiries)
        {
            DElementNum += 4;
        }
        else if (DElementNum - 4 >= 2)
        {
            DElementNum -= 4;
        }

        if (SElementNum + 4 <= numOfInquiries)
        {
            SElementNum += 4;
        }
        else if (SElementNum - 4 >= 3)
        {
           SElementNum -= 4;
        }

        Debug.Log("Viewing Other Topics");
    }
}
