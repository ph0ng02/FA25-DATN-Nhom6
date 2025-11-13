using System.Collections;
using UnityEngine;

public class BossAOESkill : MonoBehaviour
{
    [Header("AOE Skill Settings")]
    public GameObject aoePrefab;     // Prefab hiệu ứng AOE
    public Transform boss;           // Transform của boss
    public float spawnInterval = 5f; // Xuất hiện mỗi 5 giây
    public float activeDuration = 3f; // Tồn tại 3 giây
    public int damagePerTick = 2;    // Sát thương mỗi lần đốt
    public float tickInterval = 3f;  // Mỗi 3 giây gây sát thương 1 lần
    public float aoeRadius = 3f;     // Bán kính AOE

    private GameObject currentAOE;

    void Start()
    {
        if (boss == null) boss = transform;
        StartCoroutine(AOESkillLoop());
    }

    IEnumerator AOESkillLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            ActivateAOE();
            yield return new WaitForSeconds(activeDuration);
            DeactivateAOE();
        }
    }

    void ActivateAOE()
    {
        if (aoePrefab == null || currentAOE != null) return;

        // Tạo AOE ngay dưới chân boss
        currentAOE = Instantiate(
            aoePrefab,
            boss.position + Vector3.up * 0.05f,
            Quaternion.Euler(90, 0, 0) // Nằm ngang
        );
        currentAOE.transform.localScale = new Vector3(1.3f, 1f, 1.3f);
        StartCoroutine(DamageOverTime());
    }

    void DeactivateAOE()
    {
        if (currentAOE != null)
        {
            Destroy(currentAOE);
            currentAOE = null;
        }
    }

    IEnumerator DamageOverTime()
    {
        float elapsed = 0f;
        while (elapsed < activeDuration)
        {
            // Gây sát thương cho Player nếu đứng trong vùng AOE
            Collider[] hits = Physics.OverlapSphere(boss.position, aoeRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    PlayerHealth hp = hit.GetComponent<PlayerHealth>();
                    if (hp != null)
                        hp.TakeDamage(damagePerTick);
                }
            }

            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }
    }

    void Update()
    {
        // Nếu AOE đang hoạt động thì theo chân boss
        if (currentAOE != null && boss != null)
        {
            Vector3 newPos = boss.position + Vector3.up * 0.05f;
            currentAOE.transform.position = newPos;
        }
    }

    // Vẽ vùng AOE để debug
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        if (boss != null)
            Gizmos.DrawSphere(boss.position, aoeRadius);
    }
}
