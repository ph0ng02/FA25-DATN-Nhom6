using UnityEngine;

public class BoulderAccelerate : MonoBehaviour
{
    private Rigidbody rb;
    public float acceleration = 50f; // Tốc độ tăng lực liên tục

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Liên tục đẩy thêm lực về phía trước
        rb.AddForce(transform.forward * acceleration, ForceMode.Acceleration);
    }
}
