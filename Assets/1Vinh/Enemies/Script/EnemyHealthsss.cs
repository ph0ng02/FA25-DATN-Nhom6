using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHealthsss : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Health Bar UI")]
    public Slider healthSlider;      // kéo slider vào đây

    [Header("Drop Item System")]
    public GameObject[] dropItems;   // danh sách item có thể rớt
    public float dropChance = 50f;   // % tỉ lệ rớt
    public float dropForce = 3f;     // lực bắn item ra

    [Header("Rage Settings")]
    public bool rageMode = false;
    public float rageSpeedMultiplier = 1.5f;

    [Header("Knockback Settings")]
    public float knockbackForce = 4f;
    public float knockbackDuration = 0.2f;

    private Animator anim;
    private NavMeshAgent agent;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // Setup Slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    // ✅ Interface IDamageable yêu cầu
    public void TakeDamage(float amount)
    {
        TakeDamage(amount, Vector3.zero);
    }

    // Phương thức chính, có knockback
    public void TakeDamage(float amount, Vector3 hitSource)
    {
        currentHealth -= Mathf.RoundToInt(amount);

        if (anim != null)
            anim.SetTrigger("Hit");

        // Cập nhật thanh máu
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        // Kiểm tra Rage Mode
        if (!rageMode && currentHealth <= maxHealth * 0.5f)
        {
            EnterRageMode();
        }

        // Knockback khi bị đánh
        if (hitSource != Vector3.zero)
        {
            StartCoroutine(KnockbackRoutine(hitSource));
        }

        Debug.Log("Enemy bị đánh! HP còn: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void EnterRageMode()
    {
        rageMode = true;
        if (agent != null)
            agent.speed *= rageSpeedMultiplier;

        if (anim != null)
            anim.SetTrigger("Rage");

        Debug.Log("Enemy vào Rage Mode!");
    }

    private IEnumerator KnockbackRoutine(Vector3 hitSource)
    {
        if (agent == null) yield break;

        Vector3 direction = (transform.position - hitSource).normalized;
        float timer = 0f;

        while (timer < knockbackDuration)
        {
            agent.Move(direction * knockbackForce * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    void Die()
{
    if (anim != null)
        anim.SetTrigger("Die");

    if (agent != null)
        agent.isStopped = true;

    DropItem();

    if (healthSlider != null)
        Destroy(healthSlider.gameObject);

    // 👉 Chuyển scene sau khi boss chết
    StartCoroutine(LoadCutsceneAfterDelay());
}

IEnumerator LoadCutsceneAfterDelay()
{
    yield return new WaitForSeconds(2f); // chờ animation Die

    SceneManager.LoadScene("CutsceneEndgame");
}

    // Thêm vào cuối EnemyHealthsss
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    void DropItem()
    {
        float randomValue = Random.Range(0f, 100f);
        if (randomValue > dropChance) return;

        if (dropItems.Length == 0) return;

        GameObject itemToDrop = dropItems[Random.Range(0, dropItems.Length)];
        GameObject item = Instantiate(itemToDrop, transform.position, Quaternion.identity);

        if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(Vector3.up * dropForce, ForceMode.Impulse);
        }
    }
}
