using UnityEngine;

public class Combo_Gamepad : MonoBehaviour
{
    private Animator animator;

    [Header("Thời gian reset combo (giây)")]
    public float comboResetTime = 1.0f;

    [Header("Số lượng đòn trong combo")]
    public int maxCombo = 4;

    private int comboStep_X = 0;
    private int comboStep_Y = 0;

    private float lastAttackTime_X;
    private float lastAttackTime_Y;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleLightCombo();  // Combo X (nhẹ)
        HandleHeavyCombo();  // Combo Y (mạnh)
    }

    // ------------------ COMBO X ------------------
    void HandleLightCombo()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton2)) // X trên tay cầm
        {
            if (Time.time - lastAttackTime_X > comboResetTime)
                comboStep_X = 0;

            comboStep_X++;
            if (comboStep_X > maxCombo)
                comboStep_X = 1;

            animator.SetTrigger("Atk" + (comboStep_X + 8)); // Atk9–12

            lastAttackTime_X = Time.time;

            CancelInvoke(nameof(ResetComboX));
            Invoke(nameof(ResetComboX), comboResetTime);
        }
    }

    void ResetComboX()
    {
        comboStep_X = 0;
    }

    // ------------------ COMBO Y ------------------
    void HandleHeavyCombo()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton3)) // Y trên tay cầm
        {
            if (Time.time - lastAttackTime_Y > comboResetTime)
                comboStep_Y = 0;

            comboStep_Y++;
            if (comboStep_Y > maxCombo)
                comboStep_Y = 1;

            animator.SetTrigger("Atk" + (comboStep_Y + 12)); // Atk13–16

            lastAttackTime_Y = Time.time;

            CancelInvoke(nameof(ResetComboY));
            Invoke(nameof(ResetComboY), comboResetTime);
        }
    }

    void ResetComboY()
    {
        comboStep_Y = 0;
    }
}
