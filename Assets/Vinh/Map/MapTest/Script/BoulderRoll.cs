using UnityEngine;

public class BoulderRoll : MonoBehaviour
{
    public float startDelay = 2f; // chờ vài giây trước khi đá lăn
    public float rollForce = 3000f;
    private Rigidbody rb;
    private bool hasRolled = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // ban đầu chưa rơi
        Invoke(nameof(StartRolling), startDelay);
    }

    void StartRolling()
    {
        rb.isKinematic = false;
        rb.AddForce(transform.forward * rollForce);
        hasRolled = true;
    }
}
