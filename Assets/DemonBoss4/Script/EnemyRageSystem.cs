using UnityEngine;
using UnityEngine.AI;

public class EnemyRageSystem : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public NavMeshAgent agent;
    public EnemyHealthsss enemyHealth; // Script máu của bạn

    [Header("Rage Settings")]
    public bool isRageMode = false;
    public float rageSpeedMultiplier = 1.5f;
    public float rageAnimMultiplier = 1.3f;

    private void Update()
    {
        CheckRageMode();
    }

    void CheckRageMode()
    {
        if (enemyHealth == null) return; // Tránh lỗi null

        // Nếu chưa Rage và HP <= 50%
        if (!isRageMode && enemyHealth.GetCurrentHealth() <= enemyHealth.maxHealth * 0.5f)
        {
            EnterRageMode();
        }
    }

    void EnterRageMode()
    {
        isRageMode = true;

        if (agent != null)
            agent.speed *= rageSpeedMultiplier;

        if (animator != null)
            animator.speed = rageAnimMultiplier;

        animator.SetTrigger("Rage"); // Trigger animation Rage
        Debug.Log("Enemy đã vào Rage Mode!");
    }
}
