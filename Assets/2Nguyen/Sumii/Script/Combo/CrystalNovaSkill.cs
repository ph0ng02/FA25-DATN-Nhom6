using UnityEngine;

public class CrystalNovaSkill : MonoBehaviour
{
    public GameObject vfxPrefab;
    public Transform spawnPoint;
    public float cooldown = 5f;
    public float damage = 30f;
    public float radius = 3f;

    private float nextTimeCast = 0f;

    void Update()
    {
        // ❗ Đổi sang phím R
        if (Input.GetKeyDown(KeyCode.R))
        {
            CastSkill();
        }
    }

    void CastSkill()
    {
        if (Time.time < nextTimeCast) return;

        // Tạo hiệu ứng skill
        GameObject vfx = Instantiate(vfxPrefab, spawnPoint.position, spawnPoint.rotation);
        Destroy(vfx, 5f);

        // Gây sát thương
        DealDamage();

        nextTimeCast = Time.time + cooldown;
    }

    void DealDamage()
    {
        Collider[] hits = Physics.OverlapSphere(spawnPoint.position, radius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyHealth hp = hit.GetComponent<EnemyHealth>();
                if (hp != null)
                {
                    hp.TakeDamage(damage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(spawnPoint.position, radius);
    }
}
