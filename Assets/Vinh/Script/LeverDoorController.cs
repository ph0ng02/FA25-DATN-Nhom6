using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem; // ✅ Dùng cho Input System mới (Keyboard, Gamepad...)

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

    private Transform[] players; // Cả 2 người chơi

    void Start()
    {
        doorClosedPos = door.position;
        doorOpenPos = door.position + Vector3.up * doorOpenHeight;

        // Tìm cả 2 player qua Tag
        GameObject p1 = GameObject.FindGameObjectWithTag("Player1");
        GameObject p2 = GameObject.FindGameObjectWithTag("Player2");

        if (p1 != null && p2 != null)
        {
            players = new Transform[] { p1.transform, p2.transform };
        }
        else
        {
            Debug.LogWarning("⚠ Không tìm thấy Player1 hoặc Player2 trong scene!");
        }

        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (players == null || players.Length == 0) return;

        // Kiểm tra player nào đang gần nhất
        Transform nearestPlayer = GetNearestPlayer();
        float distance = Vector3.Distance(nearestPlayer.position, transform.position);

        // --- HIỂN THỊ UI ---
        if (interactText != null)
        {
            if (Gamepad.current != null)
                interactText.text = "Nhấn [A] để gạt cần";
            else
                interactText.text = "Nhấn [E] để gạt cần";

            interactText.gameObject.SetActive(distance < interactDistance && !isMoving);
        }

        // --- KIỂM TRA TƯƠNG TÁC ---
        if (distance < interactDistance && !isMoving)
        {
            bool interactPressed = false;

            // ✅ Input System mới
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                interactPressed = true;

            if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
                interactPressed = true;

            // ✅ Input System cũ (phòng trường hợp không có Input System mới)
            if (Input.GetKeyDown(interactKey) || Input.GetButtonDown("Submit") || Input.GetButtonDown("Fire1"))
                interactPressed = true;

            if (interactPressed)
                ToggleDoor();
        }
    }

    Transform GetNearestPlayer()
    {
        Transform nearest = players[0];
        float minDist = Vector3.Distance(transform.position, nearest.position);

        for (int i = 1; i < players.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, players[i].position);
            if (dist < minDist)
            {
                nearest = players[i];
                minDist = dist;
            }
        }
        return nearest;
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
