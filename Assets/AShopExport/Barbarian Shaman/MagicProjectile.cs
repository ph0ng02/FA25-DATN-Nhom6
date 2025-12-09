using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float speed = 2f;            // tốc độ bay CHẬM
    public float turnSpeed = 3f;        // tốc độ xoay từ từ
    public float lifeTime = 6f;

    private Transform target;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    private void Update()
    {
        if (target == null) return;

        // Lấy hướng tới player
        Vector3 dir = (target.position - transform.position).normalized;

        // Xoay dần dần về phía player
        Quaternion toRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);

        // Di chuyển từ từ theo hướng đang xoay
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // other.GetComponent<IDamageable>()?.TakeDamage(20);
            Destroy(gameObject);
        }
    }
}
