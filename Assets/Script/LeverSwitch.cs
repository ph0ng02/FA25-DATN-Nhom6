using UnityEngine;

public class LeverSwitch : MonoBehaviour
{
    [Header("Lever Settings")]
    public KeyCode interactKey = KeyCode.E;
    public Transform leverHandle;
    public float rotationAngle = 45f;
    public float rotateSpeed = 3f;

    [HideInInspector] public bool isActivated = false;
    public DoorSystemManager manager;

    private bool playerInRange = false;
    private Quaternion startRot;
    private Quaternion activeRot;

    void Start()
    {
        if (leverHandle != null)
        {
            startRot = leverHandle.localRotation;
            activeRot = leverHandle.localRotation * Quaternion.Euler(-rotationAngle, 0, 0);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            isActivated = !isActivated;
            manager.CheckLevers();
        }

        if (leverHandle != null)
        {
            Quaternion targetRot = isActivated ? activeRot : startRot;
            leverHandle.localRotation = Quaternion.Lerp(leverHandle.localRotation, targetRot, Time.deltaTime * rotateSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
