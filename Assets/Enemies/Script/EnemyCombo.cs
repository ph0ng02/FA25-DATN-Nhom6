using UnityEngine;
using System.Collections;

public class EnemyCombo : MonoBehaviour
{
    [System.Serializable]
    public struct ComboAttack
    {
        public string animationName;
        public float damage;
        public float windUpTime;
        public float activeTime;
        public float recoveryTime;
    }

    public ComboAttack[] comboAttacks;
    public Transform hitboxTransform;
    public Collider hitboxCollider;

    public bool IsInCombo { get; private set; }

    int currentAttackIndex = 0;
    Transform currentTarget;

    void Start()
    {
        if (hitboxCollider) hitboxCollider.enabled = false;
    }

    public int StartComboSequence(Transform target)
    {
        if (IsInCombo) return currentAttackIndex;
        if (comboAttacks.Length == 0) return 0;

        currentTarget = target;
        currentAttackIndex = (currentAttackIndex + 1) % comboAttacks.Length;
        StartCoroutine(DoComboAttack(comboAttacks[currentAttackIndex]));
        return currentAttackIndex + 1; // +1 để Animator dùng (1–3)
    }

    IEnumerator DoComboAttack(ComboAttack atk)
    {
        IsInCombo = true;

        // Wind-up
        yield return new WaitForSeconds(atk.windUpTime);

        // Active — bật hitbox
        if (hitboxCollider) hitboxCollider.enabled = true;

        yield return new WaitForSeconds(atk.activeTime);

        if (hitboxCollider) hitboxCollider.enabled = false;

        // Recovery
        yield return new WaitForSeconds(atk.recoveryTime);

        IsInCombo = false;
    }

    // Gọi từ Animation Event (đặt trong clip)
    public void EnableHitbox() { if (hitboxCollider) hitboxCollider.enabled = true; }
    public void DisableHitbox() { if (hitboxCollider) hitboxCollider.enabled = false; }

    void OnTriggerEnter(Collider other)
    {
        if (hitboxCollider && other.CompareTag("Player"))
        {
            Debug.Log($"Enemy hit player for {comboAttacks[currentAttackIndex].damage} dmg");
            // Gọi hàm nhận damage của player ở đây
        }
    }
}