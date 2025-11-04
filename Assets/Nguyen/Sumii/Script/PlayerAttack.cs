using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 1.8f;      // Tầm tấn công
    public float attackRate = 1f;         // Số đòn đánh mỗi giây
    private float nextAttackTime = 0f;    // Thời gian tấn công kế tiếp

    [Header("Damage Settings")]
    public float attackDamage = 25f;      // Sát thương gây ra (float để khớp với EnemyAI1.TakeDamage)

    [Header("References")]
    public Animator animator;             // Animator của Player
    public Transform attackPoint;         // Vị trí gây sát thương
    public LayerMask enemyLayer;          // Layer của kẻ địch

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Khi nhấn chuột trái và đủ thời gian hồi chiêu
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    private void Attack()
    {
        // Gọi animation tấn công
        if (animator != null)
            animator.SetTrigger("attack");
        else
            DealDamage(); // fallback nếu chưa có animator
    }

    // Hàm này được gọi tại Animation Event của animation tấn công
    public void DealDamage()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning("⚠️ AttackPoint chưa được gán!");
            return;
        }

        // Tìm tất cả enemy trong bán kính attackRange
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyAI1 enemyAI = enemy.GetComponent<EnemyAI1>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(attackDamage);
                Debug.Log($"🗡 Gây {attackDamage} damage lên {enemy.name}");
            }
        }
    }

    // Vẽ vùng tấn công trong Scene view để dễ chỉnh
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
