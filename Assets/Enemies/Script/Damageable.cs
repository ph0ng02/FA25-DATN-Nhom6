using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHP = 100f;
    float hp;

    void Awake() => hp = maxHP;

    public void TakeDamage(float amount)
    {
        hp -= amount;
        Debug.Log($"{gameObject.name} took {amount} dmg. HP = {hp}");
        // play hit reaction, UI, etc.
        if (hp <= 0) Die();
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        // handle death
    }
}