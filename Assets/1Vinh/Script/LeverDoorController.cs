using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem; // Dùng Input System mới

public class LeverDoorController : MonoBehaviour
{
    [Header("References")]
    public Transform door;
    public Transform leverHandle;
    public float doorOpenHeight = 3f;
    public float moveSpeed = 2f;
    public KeyCode interactKey = KeyCode.E;
    public float interactDistance = 5f;

    [Header("UI")]
    public TextMeshProUGUI interactText;

    private bool isDoorOpen = false;
    private Vector3 doorClosedPos;
    private Vector3 doorOpenPos;
    private bool isMoving = false;

    private Transform player; // 👉 CHỈ 1 PLAYER

    void Start()
    {
        doorClosedPos = door.position;
        doorOpenPos = door.position + Vector3.up * doorOpenHeight;

        // 👉 Chỉ tìm 1 player
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
            Debug.LogWarning("⚠ Không tìm thấy Player trong scene!");

        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        // --- UI ---
        if (interactText != null)
        {
            if (Gamepad.current != null)
                interactText.text = "Nhấn [A] để gạt cần";
            else
                interactText.text = "Nhấn [E] để gạt cần";

            interactText.gameObject.SetActive(distance < interactDistance && !isMoving);
        }

        // --- TƯƠNG TÁC ---
        if (distance < interactDistance && !isMoving)
        {
            bool interactPressed = false;

            // Input System mới
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                interactPressed = true;

            if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
                interactPressed = true;

            // Input system cũ
            if (Input.GetKeyDown(interactKey) || Input.GetButtonDown("Submit"))
                interactPressed = true;

            if (interactPressed)
                ToggleDoor();
        }
    }

    void ToggleDoor()
    {
        isDoorOpen = !isDoorOpen;
        StartCoroutine(MoveDoor());
        StartCoroutine(RotateLever());
        Debug.Log($"🎯 Gạt cần! Cửa {(isDoorOpen ? "đang mở" : "đang đóng")}");
    }

    System.Collections.IEnumerator MoveDoor()
    {
        isMoving = true;

        Vector3 startPos = door.position;
        Vector3 targetPos = isDoorOpen ? doorOpenPos : doorClosedPos;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            door.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        isMoving = false;
    }

    System.Collections.IEnumerator RotateLever()
    {
        Quaternion startRot = leverHandle.localRotation;
        Quaternion targetRot = isDoorOpen ? Quaternion.Euler(-45, 0, 0) : Quaternion.Euler(0, 0, 0);

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            leverHandle.localRotation = Quaternion.Lerp(startRot, targetRot, t);
            yield return null;
        }
    }
}
