using UnityEngine;

public class RockRoll : MonoBehaviour
{
    public Rigidbody rb;
    public float pushForce = 5f;

    void Start()
    {
        rb.AddForce(transform.forward * pushForce, ForceMode.Impulse);
    }
}

