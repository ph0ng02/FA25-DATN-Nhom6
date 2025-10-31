using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 1.8f;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;

    [Header("Damage Settings")]
    public int attackDamage = 25;

    [Header("References")]
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    void Update()
    {
        // Khi nhấn chuột trái để tấn công
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }
    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Attack()
    {
        // Gọi animation attack
        animator.SetTrigger("attack");
    }

    // Gọi hàm này trong Animation Event của clip Attack
    public void DealDamage()
    {
        // Kiểm tra kẻ địch trong vùng attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(attackDamage);
            }
        }
    }

    // Vẽ vùng tấn công trong Scene
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
