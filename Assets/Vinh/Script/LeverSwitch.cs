using UnityEngine;

public class LeverSwitch : MonoBehaviour
{
    [Header("Lever Settings")]
    public KeyCode interactKey = KeyCode.E;
    public Transform leverHandle;
    public float interactDistance = 3f;
    public DoorSystemManager doorManager;

    [HideInInspector] public bool isActivated = false;
    private bool canActivate = true;

    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player1 & Player2").transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            float dist = Vector3.Distance(player.position, transform.position);
            if (dist <= interactDistance && canActivate)
            {
                ActivateLever();
            }
        }
    }

    void ActivateLever()
    {
        isActivated = true;
        canActivate = false;
        leverHandle.localRotation = Quaternion.Euler(45f, 0, 0);
        Debug.Log($"{gameObject.name} activated!");

        if (doorManager != null)
        {
            doorManager.CheckLevers();
        }
    }
}