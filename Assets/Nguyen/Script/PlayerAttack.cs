using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private int damage = 25;
    [SerializeField] private float attackRadius = 1.2f;
    [SerializeField] private Transform attackPoint; // đặt một child transform trước nhân vật
    [SerializeField] private LayerMask enemyLayer;

    [Header("Cooldown")]
    [SerializeField] private float attackCooldown = 0.6f;
    private float lastAttackTime = -999f;

    [Header("Optional")]
    [SerializeField] private Animator animator;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        if (animator != null) animator.SetTrigger("Attack");

        // detect enemies
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRadius, enemyLayer);
        foreach (Collider c in hits)
        {
            EnemyHealth eh = c.GetComponentInParent<EnemyHealth>();
            if (eh != null)
            {
                Vector3 hitPoint = c.ClosestPoint(attackPoint.position);
                Vector3 dir = (c.transform.position - transform.position).normalized;
                eh.TakeDamage(damage, hitPoint, dir);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}
