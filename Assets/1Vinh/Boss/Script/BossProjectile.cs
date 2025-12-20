using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 12f;
    Vector3 target;
    float damage;

    public void Init(Vector3 targetPos, float dmg)
    {
        target = targetPos;
        damage = dmg;
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.2f)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<HealthManagement>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
