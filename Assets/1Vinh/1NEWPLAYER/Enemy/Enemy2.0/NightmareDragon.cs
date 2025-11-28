using UnityEngine;
using System.Collections;

public class NightmareDragon : MonoBehaviour, IDamageable
{
    [Header("Máu")]
    public int HP = 100;

    [Header("Animator")]
    public Animator animator;   

    [Header("Tấn công")]
    [SerializeField] private float attackDamage = 30f;
    [SerializeField] private float attackCooldown = 0.5f;
    private float lastAttackTime = 0f;

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
            animator.SetBool("IsDie", true);
            Debug.Log("▶️ Enemy chết, play animation die");

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
        // Lấy thời lượng animation die
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animTime = stateInfo.length;

        // Đợi animation chết chạy xong
        yield return new WaitForSeconds(animTime);

        Debug.Log("⏳ Animation die xong, chờ thêm 10s...");

        // ❗❗ BƯỚC 3 — Tăng kill count
        QuestData.instance.killCount++;
        Debug.Log("Kill Count: " + QuestData.instance.killCount);

        if (QuestData.instance.killCount >= QuestData.instance.killTarget)
        {
            QuestData.instance.state = QuestState.Completed;
            Debug.Log("🎉 Nhiệm vụ đã hoàn thành!");
        }

        yield return new WaitForSeconds(10f);

        Debug.Log("💥 Destroy enemy sau 10s");
        Destroy(gameObject);
    }
}
