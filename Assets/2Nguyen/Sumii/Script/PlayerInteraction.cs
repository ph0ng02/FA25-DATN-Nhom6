using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Camera playerCamera;
    public float rayDistance = 3f;
    public LayerMask interactableLayer;

    private Interactable currentInteractable;

    void Awake()
    {
        // 🔹 Nếu chưa gán trong Inspector → tự động tìm Camera chính
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        // 🔸 Nếu vẫn không có camera, ngừng chạy để tránh lỗi
        if (playerCamera == null)
        {
            Debug.LogError("❌ PlayerInteraction: Không tìm thấy camera! Hãy gán Camera vào playerCamera.");
            return;
        }

        DetectInteractable();

        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact();
        }
    }

    void DetectInteractable()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;
                    Debug.Log("👉 " + interactable.interactionText);
                }
                return;
            }
        }

        currentInteractable = null;
    }
}
