using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("References")]
    public Transform target;        // Player
    public Transform pivot;         // Điểm xoay camera
    public Transform cam;           // Main Camera
    public CharacterController controller; // Thêm CharacterController củ player

    [Header("Settings")]
    public float sensitivity = 3f;  
    public float distance = 5f;     
    public float pitchMin = -30f;
    public float pitchMax = 70f;
    public float moveSpeed = 5f;    
    public float rotateSpeed = 10f; 

    private float yaw;
    private float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null || pivot == null || cam == null) return;

        // --- Xử lý chuột xoay camera ---
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        pivot.rotation = Quaternion.Euler(pitch, yaw, 0);
        pivot.position = target.position;
        cam.localPosition = new Vector3(0, 0, -distance);

        // --- Xử lý di chuyển ---
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(horizontal, 0, vertical).normalized;

        if (moveDir.magnitude >= 0.1f)
        {
            Vector3 camForward = cam.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = cam.right;
            camRight.y = 0;
            camRight.Normalize();

            Vector3 moveDirWorld = camForward * moveDir.z + camRight * moveDir.x;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirWorld);
            target.rotation = Quaternion.Lerp(target.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            controller.Move(moveDirWorld * moveSpeed * Time.deltaTime);
        }
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
