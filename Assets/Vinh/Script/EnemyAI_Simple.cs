using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Simple : MonoBehaviour
{
    [Header("Ranges")]
    public float detectRange = 15f;
    public float wakeUpRange = 8f;
    public float attackRange = 10f;

    [Header("Attack Settings")]
    public float attackCooldown = 3f;
    public int attackDamage = 15;

    [Header("Animation Settings")]
    [Range(0.1f, 2f)] public float baseAnimSpeed = 1f;
    [Range(0.1f, 1f)] public float attackSlowFactor = 0.7f;
    public float speedSmooth = 5f;

    [Header("Animation Stability")]
    public float walkStartThreshold = 0.3f; // 🔹 khi nào chuyển sang walk
    public float walkStopThreshold = 0.1f;  // 🔹 khi nào dừng lại về idle

    private Transform target;
    private Animator anim;
    private NavMeshAgent agent;
    private float lastAttackTime;

    private bool isAwake = false;
    private bool isDead = false;
    private bool isSleeping = true;
    private float currentAnimSpeed = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (agent != null && agent.isOnNavMesh)
            agent.isStopped = true;
        else
            Debug.LogWarning($"{name} chưa nằm trên NavMesh!");

        anim.speed = baseAnimSpeed;
        anim.Play("Sleep", 0, 0f);
        anim.ResetTrigger("AwakeTrig");
    }

    void Update()
    {
        if (isDead) return;

        GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
        GameObject player2 = GameObject.FindGameObjectWithTag("Player2");

        float dist1 = player1 ? Vector3.Distance(transform.position, player1.transform.position) : Mathf.Infinity;
        float dist2 = player2 ? Vector3.Distance(transform.position, player2.transform.position) : Mathf.Infinity;

        target = dist1 < dist2 ? player1?.transform : player2?.transform;
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        // 😴 Thức dậy
        if (isSleeping && distance <= wakeUpRange)
        {
            WakeUp();
            return;
        }

        if (!isAwake) return;

        // 🔥 Khi tỉnh
        if (distance <= attackRange)
        {
            agent.isStopped = true;

            if (Time.time - lastAttackTime > attackCooldown)
            {
                lastAttackTime = Time.time;

                int attackType = Random.Range(0, 2);
                if (attackType == 0)
                    anim.SetTrigger("AttackTrig");
                else
                    anim.SetTrigger("ClawTrig");

                anim.speed = baseAnimSpeed * attackSlowFactor;
                Invoke(nameof(ResetAnimSpeed), 2f);
                Debug.Log($"{name} tấn công người chơi!");
            }

            SetStableSpeed(0f);
        }
        else if (distance <= detectRange)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            SetStableSpeed(agent.velocity.magnitude);
        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
            SetStableSpeed(0f);
        }
    }

    void SetStableSpeed(float agentSpeed)
    {
        // 🔹 Ổn định giữa walk và idle
        if (currentAnimSpeed < walkStartThreshold && agentSpeed > walkStartThreshold)
            currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, agentSpeed, Time.deltaTime * speedSmooth);
        else if (currentAnimSpeed > walkStopThreshold && agentSpeed < walkStopThreshold)
            currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, 0f, Time.deltaTime * speedSmooth);
        else
            currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, agentSpeed, Time.deltaTime * speedSmooth);

        anim.SetFloat("Speed", currentAnimSpeed);
    }

    void ResetAnimSpeed() => anim.speed = baseAnimSpeed;

    void WakeUp()
    {
        if (isSleeping)
        {
            isSleeping = false;
            isAwake = true;
            anim.SetTrigger("AwakeTrig");
            Debug.Log($"{name} woke up!");
        }
    }

    public void OnGetHit()
    {
        if (isDead) return;
        anim.SetTrigger("HitTrigger");
    }

    public void OnDie()
    {
        if (isDead) return;
        isDead = true;
        agent.isStopped = true;
        anim.SetTrigger("DieTrigger");
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        anim.SetTrigger("HitTrigger");
        EnemyHealth_Simple hp = GetComponent<EnemyHealth_Simple>();

        if (hp != null)
        {
            hp.TakeDamage(dmg);
            if (hp.currentHealth <= 0)
                OnDie();
        }
    }
}
