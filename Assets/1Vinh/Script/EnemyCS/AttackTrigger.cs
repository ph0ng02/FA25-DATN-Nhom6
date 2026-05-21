using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public bool playerInAttackRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInAttackRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInAttackRange = false;
    }
}
