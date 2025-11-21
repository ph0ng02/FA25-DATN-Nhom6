using UnityEngine;

public class EnemyHealth_Simple : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private EnemyAI_Simple enemyAI;

    void Start()
    {
        currentHealth = maxHealth;
        enemyAI = GetComponent<EnemyAI_Simple>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (enemyAI != null)
        {
            enemyAI.OnGetHit();
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (enemyAI != null)
            {
                enemyAI.OnDie();
            }
        }
    }
}
