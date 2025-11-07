using UnityEngine;

public class PlayerControllerJoystick : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform cameraTransform;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal2");
        float vertical = Input.GetAxis("Vertical2");

        Vector3 move = new Vector3(horizontal, 0, vertical);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0;

        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

        // Xoay player theo hướng di chuyển
        if (move.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);
        }
    }
}
