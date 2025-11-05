using UnityEngine;

public class PlayerHealth_Simple : MonoBehaviour
{
    public int maxHealth = 100;
    int current;

    void Start()
    {
        current = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        current -= amount;
        Debug.Log(name + " took " + amount + " damage. HP left: " + current);
        if (current <= 0) Debug.Log(name + " died!");
    }
}
