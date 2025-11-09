using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController_GamePad : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Combat Settings")]
    public float rollDuration = 0.6f;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleActions();
    }

    void HandleActions()
    {
        // --- Attack thường (Fire3) ---
        if (Input.GetButtonDown("Fire3")) // Joystick Button 2
            animator.SetTrigger("Attack");

        // --- Combo (Fire4) ---
        if (Input.GetButtonDown("Fire4")) // Joystick Button 3
            animator.SetTrigger("Attack1");

        // --- Jump ---
        if (Input.GetButtonDown("Jump"))
            animator.SetBool("IsJumping", true);
        else
            animator.SetBool("IsJumping", false);

        // --- Roll ---
        if (Input.GetButtonDown("Roll"))
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
