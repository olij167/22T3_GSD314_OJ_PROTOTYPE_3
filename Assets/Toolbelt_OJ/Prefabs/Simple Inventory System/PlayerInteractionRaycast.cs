using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractionRaycast : MonoBehaviour
{

    [SerializeField] private KeyCode selectInput = KeyCode.E;

    [SerializeField] private float reachDistance = 5f;

    [SerializeField] private GameObject selectedObject, selectedNPC;

    [SerializeField] private Inventory inventory;

    [SerializeField] private StartDialogue initiateDialogue;

    [SerializeField] private GameObject interactIndicator;

    [SerializeField] private TextMeshProUGUI checkInventoryIndicator;
    
    [SerializeField] private float inventoryIndicatorDisplayTime = 7.5f;
     private float inventoryIndicatorDisplayTimeReset;

    [SerializeField] private bool displayInventoryIndicator;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private bool isItem, isNPC;


    void Start()
    {
        interactIndicator.SetActive(false);

        checkInventoryIndicator.text = "New Item! Press '" + selectInput + "' to check your inventory";

        checkInventoryIndicator.enabled = false;
        inventoryIndicatorDisplayTimeReset = inventoryIndicatorDisplayTime;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachDistance) && !initiateDialogue.dialogueSystem.enabled) //Camera.main.transform.position, Camera.main.transform.forward
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.magenta);

            if (hit.transform.GetComponent<ItemInWorld>())
            {
                isItem = true;
                selectedObject = hit.transform.gameObject;
                Debug.Log("hit = " + selectedObject);
                interactIndicator.SetActive(true);
            }
            else
            {
                isItem = false;
            }

            if (hit.transform.GetComponent<NPCBrain>())
            {
                isNPC = true;
                selectedObject = hit.transform.gameObject;
                Debug.Log("hit = " + selectedObject);
                interactIndicator.SetActive(true);
                
            }
            else
            {
                isNPC = false;
            }

            if (selectedObject != null && Input.GetKeyDown(selectInput))
            {
                if (isNPC)
                {
                    SelectNPC();
                    selectedObject.transform.GetComponent<NPCBrain>().speakingToPlayer = true;
                }

                if (isItem)
                {
                    PickUpItem();
                }
                
                StartCoroutine(CheckInventoryIndicator());
            }
 
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
            selectedObject = null;
            interactIndicator.SetActive(false);
        }

        
    }

    public void SelectNPC()
    {
        if (selectedObject != null && selectedObject.GetComponent<NPCBrain>())
        {
            if (selectedObject.GetComponent<NPCBrain>().npcInfo != null)
            {
                Debug.Log("NPC Selected: " + selectedObject.GetComponent<NPCBrain>().npcInfo.npcName);
                initiateDialogue.EnterDialogue(selectedObject.GetComponent<NPCBrain>().npcInfo);
            }
        }
    }

    public void PickUpItem()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();

            audioSource.PlayOneShot(selectedObject.GetComponent<ItemInWorld>().itemCollectedAudio);
        }

        if (selectedObject.GetComponent<ItemInWorld>().hasDialogue)
        {
            selectedObject.GetComponent<ItemInWorld>().unlockNewDialogue.enabled = true;
        }

        inventory.AddItemToInventory(selectedObject.GetComponent<ItemInWorld>().item);
        if (inventory.inventory.Contains(selectedObject.GetComponent<ItemInWorld>().item))
        {
            Destroy(selectedObject);
            selectedObject = null;
            interactIndicator.SetActive(false);
            displayInventoryIndicator = true;
        }
    }

    public IEnumerator CheckInventoryIndicator()
    {
        if (displayInventoryIndicator)
        {
            checkInventoryIndicator.enabled = true;
            inventoryIndicatorDisplayTime -= Time.deltaTime;

            if (inventoryIndicatorDisplayTime >= inventoryIndicatorDisplayTimeReset - 2f)
            {
                checkInventoryIndicator.alpha = Mathf.Lerp(0f, 1f, inventoryIndicatorDisplayTimeReset - inventoryIndicatorDisplayTime);

            }

            if (inventoryIndicatorDisplayTime <= 2f)
            {
                checkInventoryIndicator.alpha = Mathf.Lerp(0f, 1f, inventoryIndicatorDisplayTime);
            }

            if (inventoryIndicatorDisplayTime <= 0f)
            {

                displayInventoryIndicator = false;
                inventoryIndicatorDisplayTime = inventoryIndicatorDisplayTimeReset;
                displayInventoryIndicator = false;
            }
        }

        yield return null;
    }
}
