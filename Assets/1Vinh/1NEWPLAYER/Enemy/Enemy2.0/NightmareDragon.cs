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
    private Transform detectedPlayer;   // player được phát hiện từ vùng tầm nhìn
    private bool playerInVision = false;

    [Header("Item Drop")]
    public GameObject healthPickupPrefab;
    public float dropForce = 3f;

    private bool isDead = false;

    // ---------- NEW: AI FOLLOW PLAYER ----------
    [Header("AI Follow Player")]
    public float viewRange = 10f;      // tầm nhìn
    public float moveSpeed = 3f;       // tốc độ dí
    private Transform player;          // lưu player
    private Rigidbody rb;              // di chuyển vật lý
    // ------------------------------------------

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        HP -= Mathf.RoundToInt(amount);
        if (HP <= 0)
        {
            isDead = true;
            animator.SetBool("IsDie", true);

            QuestManager.Instance.AddKill();
            if (QuestUI.Instance != null)
                QuestUI.Instance.UpdateUI();

            StartCoroutine(WaitAndDestroy());
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }

    public void PlayerDetected(Transform player)
    {
        detectedPlayer = player;
        playerInVision = true;
    }

    public void LosePlayer()
    {
        playerInVision = false;
        detectedPlayer = null;

        animator.SetBool("isRunning", false); // dừng animation chạy
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (!playerInVision || detectedPlayer == null)
        {
            animator.SetBool("isRunning", false);
            return;
        }

        // rồng dí theo player detected
        Vector3 dir = (detectedPlayer.position - transform.position).normalized;

        Quaternion look = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, 5f * Time.deltaTime);

        rb.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);

        animator.SetBool("isRunning", true);
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
        yield return new WaitForSeconds(3f);

        // DROP ITEM
        if (healthPickupPrefab != null)
        {
            GameObject drop = Instantiate(healthPickupPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);

            if (drop.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.AddForce(Vector3.up * dropForce, ForceMode.Impulse);
            }
        }

        Destroy(gameObject);
    }
}
