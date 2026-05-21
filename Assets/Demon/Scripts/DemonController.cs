using UnityEngine;

public class DemonController : MonoBehaviour
{
    private Animator animator;
    private Transform player;

    [Header("Settings")]
    public float detectRange = 12f;
    public float attackRange = 2.5f;
    public float moveSpeed = 3f;
    public float rotateSpeed = 8f;
    public float timeBetweenAttacks = 1.5f;

    private bool alreadyAttacked = false;
    private bool isDead = false;

    [Header("Attack Points")]
    public Transform attackPoint1;   // vị trí spawn VFX 1
    public Transform attackPoint2;   // vị trí spawn VFX 2

    [Header("VFX")]
    public GameObject attackVFX1;
    public GameObject attackVFX2;

    [Header("Attack Damage")]
    public float attackDamage = 20f;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Player ngoài tầm phát hiện → Idle
        if (distance > detectRange)
        {
            animator.SetBool("Walk", false);
            return;
        }

        // Player trong tầm phát hiện nhưng ngoài tầm đánh → Đi tới player
        if (distance > attackRange)
        {
            animator.SetBool("Walk", true);

            Vector3 lookDir = (player.position - transform.position).normalized;
            lookDir.y = 0;

            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);

            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Trong tầm đánh
            animator.SetBool("Walk", false);

            Vector3 lookDir = (player.position - transform.position).normalized;
            lookDir.y = 0;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(lookDir),
                Time.deltaTime * rotateSpeed
            );

            if (!alreadyAttacked)
            {
                animator.SetTrigger("Attack");
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }

    // Animation Event gọi hàm này tại đúng frame chém
    public void SpawnAttackVFX()
    {
        SpawnOneVFX(attackVFX1, attackPoint1);
        SpawnOneVFX(attackVFX2, attackPoint2);
    }

    private void SpawnOneVFX(GameObject vfxPrefab, Transform point)
    {
        if (vfxPrefab == null || point == null) return;

        GameObject vfx = Instantiate(vfxPrefab, point.position, point.rotation);

        // Nếu prefab có script DamageOnHit thì gán sát thương
        DamageOnHit dmg = vfx.GetComponent<DamageOnHit>();
        if (dmg != null)
        {
            dmg.damage = attackDamage;
        }

        // Destroy VFX sau khi chạy xong
        ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
        if (ps != null)
            Destroy(vfx, ps.main.duration);
        else
            Destroy(vfx, 1f);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Die");

        Destroy(gameObject, 3f);
    }
}
