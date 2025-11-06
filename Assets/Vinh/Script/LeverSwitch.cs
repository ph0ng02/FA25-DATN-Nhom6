using UnityEngine;
using UnityEngine.InputSystem; // ✅ Cho Input System mới (Keyboard, Gamepad...)

public class LeverSwitch : MonoBehaviour
{
    [Header("Lever Settings")]
    public KeyCode interactKey = KeyCode.E;
    public Transform leverHandle;
    public float interactDistance = 3f;
    public DoorSystemManager doorManager;

    [HideInInspector] public bool isActivated = false;
    private bool canActivate = true;

    private Transform[] players; // Cả 2 người chơi

    void Start()
    {
        // ✅ Tìm Player1 và Player2 qua Tag
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
    }

    void Update()
    {
        if (players == null || players.Length == 0) return;

        Transform nearestPlayer = GetNearestPlayer();
        float dist = Vector3.Distance(nearestPlayer.position, transform.position);

        // --- Kiểm tra thao tác ---
        bool interactPressed = false;

        // Input System mới (Keyboard + Gamepad)
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            interactPressed = true;

        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
            interactPressed = true; // Nút A (Xbox) / X (PlayStation)

        // Input System cũ (dự phòng)
        if (Input.GetKeyDown(interactKey) || Input.GetButtonDown("Submit") || Input.GetButtonDown("Fire1"))
            interactPressed = true;

        if (interactPressed && dist <= interactDistance && canActivate)
        {
            ActivateLever();
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

    void ActivateLever()
    {
        isActivated = true;
        canActivate = false;

        // Xoay cần
        if (leverHandle != null)
            leverHandle.localRotation = Quaternion.Euler(45f, 0, 0);

        Debug.Log($"{gameObject.name} activated!");

        // Gọi qua Door Manager
        if (doorManager != null)
        {
            doorManager.CheckLevers();
        }
    }
}
