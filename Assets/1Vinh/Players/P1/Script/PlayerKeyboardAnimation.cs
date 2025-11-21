using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerKeyboardAnimation : MonoBehaviour
{
    public Animator animator;
    public float rollDuration = 0.6f;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleKeyboardAnimation();
    }

    void HandleKeyboardAnimation()
    {
        // --- Attack thường (phím I) ---
        if (Input.GetKeyDown(KeyCode.I))
            animator.SetTrigger("Attack");

        // --- Combo (phím Q) ---
        if (Input.GetKeyDown(KeyCode.Q))
            animator.SetTrigger("Attack1");

        // --- Jump ---
        if (Input.GetKeyDown(KeyCode.Space))
            animator.SetBool("IsJumping", true);
        else if (Input.GetKeyUp(KeyCode.Space))
            animator.SetBool("IsJumping", false);

        // --- Roll ---
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCoroutine(PerformRoll());

        // --- Take Damage ---
        if (Input.GetKeyDown(KeyCode.T))
            animator.SetBool("IsTakingDamage", true);
        else
            animator.SetBool("IsTakingDamage", false);

        // --- Dead ---
        if (Input.GetKeyDown(KeyCode.Y))
            animator.SetBool("IsDead", true);
    }

    private System.Collections.IEnumerator PerformRoll()
    {
        animator.SetBool("IsRolling", true);
        float elapsed = 0f;

        while (elapsed < rollDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("IsRolling", false);
    }
}
