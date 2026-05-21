using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 10f;
    public float rotateSpeed = 6f;
    public float damage = 20f;
    public float lifeTime = 5f;

    private Rigidbody rb;
    private Vector3 targetPos;      // <-- VECTOR3
    private bool fired = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    // FIRE NHẬN VECTOR3 CHUẨN
    public void Fire(Vector3 target)
    {
        targetPos = target;
        fired = true;
    }

    void FixedUpdate()
    {
        if (!fired) return;

        Vector3 direction = (targetPos - transform.position).normalized;

        // Xoay về phía mục tiêu
        Vector3 rotateDir = Vector3.RotateTowards(
            transform.forward,
            direction,
            rotateSpeed * Time.fixedDeltaTime,
            0f
        );

        transform.rotation = Quaternion.LookRotation(rotateDir);

        // Di chuyển tới mục tiêu
        rb.linearVelocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage((int)damage, 5f, transform.forward);
            }

            Destroy(gameObject);
        }

        if (!other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
