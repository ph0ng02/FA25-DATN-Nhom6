using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float speed = 6f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    public float groundDistance = 0.4f;
    private Vector3 velocity;
    private bool isGrounded;

    void Update()
    {
        // Kiểm tra va chạm với đất
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // giữ nhân vật dính đất
        }

        // Di chuyển WASD
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Nhảy
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Trọng lực
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
