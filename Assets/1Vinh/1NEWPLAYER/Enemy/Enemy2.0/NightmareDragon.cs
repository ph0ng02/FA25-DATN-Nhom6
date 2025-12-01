using UnityEngine;
using System.Collections;

public class NightmareDragon : MonoBehaviour, IDamageable
{
    [Header("Máu")]
    public int HP = 100;

    [Header("Animator")]
    public Animator animator;   

    [Header("Tấn công")]
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private float attackCooldown = 0.5f;
    private float lastAttackTime = 0f;

    [Header("Item Drop")]
    public GameObject healthPickupPrefab;   // prefab máu rơi ra
    public float dropForce = 3f;            // lực nảy lên nhẹ

    private bool isDead = false;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void TakeDamage(float amount)
{
    if (isDead) return;

    HP -= Mathf.RoundToInt(amount);
    if (HP <= 0)
    {
        isDead = true;
        animator.SetBool("IsDie", true); // ✅ play animation die
        Debug.Log("▶️ Enemy chết, play animation die");

        // ✅ Thêm kill vào QuestManager
        QuestManager.Instance.AddKill();

        // ✅ Cập nhật UI nếu đang hiển thị
        if (QuestUI.Instance != null)
            QuestUI.Instance.UpdateUI();

        StartCoroutine(WaitAndDestroy());
    }
    else
    {
        animator.SetTrigger("damage");
    }
}

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        if (Time.time - lastAttackTime < attackCooldown) return;

        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            Debug.Log("Zombie tấn công player bằng trigger!");
            target.TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }
    }

    private IEnumerator WaitAndDestroy()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animTime = stateInfo.length;

        yield return new WaitForSeconds(animTime);

        Debug.Log("⏳ Animation die xong, chờ thêm 10s...");
        yield return new WaitForSeconds(10f);

        // 🎁 DROP MÁU
        if (healthPickupPrefab != null)
        {
            GameObject drop = Instantiate(healthPickupPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);

            if (drop.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.AddForce(Vector3.up * dropForce, ForceMode.Impulse);
            }
        }

        Debug.Log("💥 Destroy enemy sau 10s");
        Destroy(gameObject);
    }
}
