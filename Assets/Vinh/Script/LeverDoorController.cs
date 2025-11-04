using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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
    private Transform playerCam;

    void Start()
    {
        doorClosedPos = door.position;
        doorOpenPos = door.position + Vector3.up * doorOpenHeight;

        // Lấy camera an toàn (kể cả khi chưa gán)
        if (Camera.main != null)
        {
            playerCam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("⚠ Không tìm thấy Camera có tag 'MainCamera'!");
        }

        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }
    public void OnInteract()
    {
        if (!isMoving)
        {
            ToggleDoor();
        }
    }

    bool isTrigger;
    void Update()
    {
        //Kiểm tra nhấn E
       if (isTrigger && Input.GetKeyDown(interactKey) && !isMoving)
        {
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

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.TryGetComponent(out PlayerAnimationControl controll))
        {
            isTrigger = true;
            if (interactText != null)
                interactText.gameObject.SetActive(true && !isMoving);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerAnimationControl controll))
        {
            isTrigger = false;
            if (interactText != null)
                interactText.gameObject.SetActive(false);
        }
    }
}