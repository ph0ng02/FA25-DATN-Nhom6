using UnityEngine;

public class VisionRange : MonoBehaviour
{
    private EnemyAIs enemyAI;

    private void Start()
    {
        enemyAI = GetComponentInParent<EnemyAIs>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAI.PlayerInVision(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAI.PlayerInVision(false);
        }
    }
}
