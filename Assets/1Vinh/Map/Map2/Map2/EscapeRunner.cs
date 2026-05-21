using UnityEngine;

public class EscapeRunner : MonoBehaviour
{
    public float forwardSpeed = 12f;
    public float laneDistance = 3f;

    private int lane = 1;
    private float verticalVelocity;
    public float jumpForce = 8f;
    public float gravity = 20f;

    private CharacterController controller;

    [Header("Slide Settings")]
    public float slideDuration = 0.8f; // thời gian slide
    public float slideHeight = 0.5f;   // chiều cao khi trượt
    private float originalHeight;
    private bool isSliding = false;
    private float slideTimer = 0f;

    [Header("Animator")]
    public Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
    }

    void Update()
    {
        Vector3 move = Vector3.forward * forwardSpeed;

        // ----- ĐỔI LANE -----
        if (Input.GetKeyDown(KeyCode.A)) lane = Mathf.Max(0, lane - 1);
        if (Input.GetKeyDown(KeyCode.D)) lane = Mathf.Min(2, lane + 1);

        float targetX = (lane - 1) * laneDistance;
        move.x = (targetX - transform.position.x) * 10f;

        // ----- JUMP -----
        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isSliding)
            {
                verticalVelocity = jumpForce;
                if (animator != null)
                    animator.SetTrigger("Jump");
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        move.y = verticalVelocity;

        // ----- SLIDE -----
        if (Input.GetKeyDown(KeyCode.LeftControl) && controller.isGrounded && !isSliding)
        {
            StartSlide();
        }

        if (isSliding)
        {
            slideTimer += Time.deltaTime;
            if (slideTimer >= slideDuration)
            {
                EndSlide();
            }
        }

        controller.Move(move * Time.deltaTime);

        // ----- Animator chạy liên tục -----
        if (animator != null)
            animator.SetBool("IsRunning", true);
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = 0f;
        controller.height = slideHeight;
        if (animator != null)
            animator.SetTrigger("Slide");
    }

    void EndSlide()
    {
        isSliding = false;
        controller.height = originalHeight;
    }
}
