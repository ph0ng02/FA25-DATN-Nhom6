//using UnityEngine;

//[RequireComponent(typeof(Collider))]
//public class NPCInteractTrigger : MonoBehaviour
//{
    
//    public NPCDialogueNV2 dialogueScript; // kéo component NPCDialogueNV2 vào đây
//    public KeyCode interactKey = KeyCode.E;

//    bool playerInRange = false;

//    void Reset()
//    {
//        // ensure collider is trigger
//        Collider c = GetComponent<Collider>();
//        if (c != null) c.isTrigger = true;
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            playerInRange = true;
//            Debug.Log("[NPCInteractTrigger] Player in range for " + gameObject.name);
//            // optional show "Press E to talk" UI here
//        }
//    }

//    void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            playerInRange = false;
//            Debug.Log("[NPCInteractTrigger] Player left range for " + gameObject.name);
//            // hide press E UI
//        }
//    }

//    void Update()
//    {
//        if (playerInRange && Input.GetKeyDown(interactKey))
//        {
//            if (dialogueScript != null)
//            {
//                Debug.Log("[NPCInteractTrigger] E pressed, starting dialogue on " + gameObject.name);
//                dialogueScript.StartDialogue();
//            }
//            else
//            {
//                Debug.LogWarning("[NPCInteractTrigger] dialogueScript not assigned on " + gameObject.name);
//            }
//        }
//    }
//}
