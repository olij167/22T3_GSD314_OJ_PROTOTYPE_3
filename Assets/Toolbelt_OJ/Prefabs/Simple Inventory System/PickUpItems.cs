using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpItems : MonoBehaviour
{

    [SerializeField] private float reachDistance;

    [SerializeField] private GameObject selectedObject;

    [SerializeField] private Inventory inventory;

    [SerializeField] private GameObject pickUpIndicator;

    [SerializeField] private TextMeshProUGUI checkInventoryIndicator;
    
    [SerializeField] private float inventoryIndicatorDisplayTime;
     private float inventoryIndicatorDisplayTimeReset;

    [SerializeField] private bool displayInventoryIndicator;

    [SerializeField] private AudioSource audioSource;


    void Start()
    {
        pickUpIndicator.SetActive(false);
        checkInventoryIndicator.enabled = false;
        inventoryIndicatorDisplayTimeReset = inventoryIndicatorDisplayTime;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachDistance)) //Camera.main.transform.position, Camera.main.transform.forward
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.magenta);

            if (hit.transform.CompareTag("Item"))
            {
                selectedObject = hit.transform.gameObject;
                Debug.Log("hit = " + selectedObject);
                pickUpIndicator.SetActive(true);
            }
            else
            {
                selectedObject = null;
                pickUpIndicator.SetActive(false);
            }

            if (selectedObject != null && Input.GetKeyDown(KeyCode.E))
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
                    pickUpIndicator.SetActive(false);
                    displayInventoryIndicator = true;
                }
            }
 
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
            selectedObject = null;
            pickUpIndicator.SetActive(false);
        }

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
    }
}
