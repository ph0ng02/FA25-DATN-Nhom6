using UnityEngine;
using TMPro;

public class InteractionPlayer : MonoBehaviour
{
    [Header("Cài đặt tương tác")]
    public float interactDistance = 5f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask interactLayer;

    [Header("UI hiển thị")]
    public TextMeshProUGUI interactText;

    private Camera playerCam;
    private RaycastHit hit;

    void Start()
    {
        playerCam = Camera.main;
        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerCam == null) return;

        // Bắn tia (raycast) từ camera ra trước mặt player
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, interactDistance, interactLayer))
        {
            // Nếu có đối tượng có thể tương tác
            if (interactText != null)
            {
                interactText.text = "Nhấn [E] để tương tác";
                interactText.gameObject.SetActive(true);
            }

            // Khi nhấn phím E
            if (Input.GetKeyDown(interactKey))
            {
                // Gọi hàm tương tác trên object
                hit.collider.SendMessage("OnInteract", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            // Không thấy vật thể nào có thể tương tác
            if (interactText != null)
                interactText.gameObject.SetActive(false);
        }
    }
}
