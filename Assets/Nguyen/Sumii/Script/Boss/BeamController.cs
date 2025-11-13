using UnityEngine;

public class BeamController : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f;
    public int damage = 50;
    private Transform target;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
        transform.LookAt(target);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Gây damage hoặc hiệu ứng ở đây
            Debug.Log("Player trúng beam!");
            Destroy(gameObject);
        }
    }
}
