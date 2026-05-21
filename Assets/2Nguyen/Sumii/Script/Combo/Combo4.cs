using UnityEngine;

public class Combo4 : MonoBehaviour
{
    private Animator animator;            // Để private cho an toàn
    public float comboResetTime = 1.0f;
    private int currentAttack = 0;
    private float lastAttackTime;

    void Start()
    {
        // ✅ Tự động tìm Animator trong Player hoặc con của Player
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Time.time - lastAttackTime > comboResetTime)
                currentAttack = 0;

            currentAttack++;
            if (currentAttack > 4)
                currentAttack = 1;

            animator.SetTrigger("Atk" + (currentAttack + 12)); // Atk13 → 16
            lastAttackTime = Time.time;

            CancelInvoke(nameof(ResetCombo));
            Invoke(nameof(ResetCombo), comboResetTime);
        }
    }

    void ResetCombo()
    {
        currentAttack = 0;
    }
}
