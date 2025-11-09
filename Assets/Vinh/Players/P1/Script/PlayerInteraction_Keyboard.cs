using UnityEngine;

public class PlayerInteraction_Keyboard : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 3f;        // Khoảng cách tương tác
    public LayerMask npcLayer;              // Layer của NPC
    public KeyCode interactKey = KeyCode.E; // Phím tương tác

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            RaycastHit hit;
            Vector3 origin = transform.position + Vector3.up * 1.5f;
            Vector3 direction = transform.forward;

            if (Physics.Raycast(origin, direction, out hit, interactRange, npcLayer))
            {
                NPCDialogue npc = hit.collider.GetComponent<NPCDialogue>();
                if (npc != null)
                {
                    npc.Interact();
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Gizmos.DrawRay(origin, transform.forward * interactRange);
    }
}
