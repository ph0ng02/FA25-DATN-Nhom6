using UnityEngine;
using System.Collections;

public class MovingDamageWall : MonoBehaviour
{
    [Header("Movement")]
    public float moveDistance = 4f;
    public float moveSpeed = 40f;
    public bool randomDirection = true;

    [Header("Random Offset")]
    public float randomTimeOffset = 5f;

    [Header("Damage")]
    public float damage = 20f;
    public float damageCooldown = 1f;

    private Vector3 startPos;
    private float timeOffset;
    private int direction = 1;
    private bool canDamage = true;

    void Start()
    {
        startPos = transform.position;

        // ⏱ Random lệch nhịp
        timeOffset = Random.Range(0f, randomTimeOffset);

        // 🔁 Random hướng
        if (randomDirection)
            direction = Random.value > 0.5f ? 1 : -1;

        // ⚡ Random tốc độ nhẹ
        moveSpeed *= Random.Range(0.8f, 1.2f);
    }

    void Update()
    {
        float z = Mathf.PingPong((Time.time + timeOffset) * moveSpeed, moveDistance);
        z = (z - moveDistance / 2f) * direction;

        transform.position = startPos + new Vector3(0, 0, z);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!canDamage) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            HealthManagement hp = collision.gameObject.GetComponent<HealthManagement>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
                StartCoroutine(DamageCooldown());
            }
        }
    }

    IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}
