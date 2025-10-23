using UnityEngine;

public class Combo3 : MonoBehaviour
{
    private Animator animator;
    private int comboStep = 0;
    private float comboTimer = 0f;

    [Header("Thời gian reset combo (sau khi ngưng bấm R)")]
    public float comboDelay = 1.0f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>(); // hoặc GetComponent<Animator>() nếu Animator nằm cùng object
    }

    void Update()
    {
        // Nhấn phím R để tấn công combo
        if (Input.GetKeyDown(KeyCode.R))
        {
            ComboAttack();
        }

        // Nếu combo dừng quá lâu → reset lại
        if (comboStep > 0 && Time.time - comboTimer > comboDelay)
        {
            ResetCombo();
        }
    }

    void ComboAttack()
    {
        comboTimer = Time.time;

        // Tắt toàn bộ Atk trước khi bật mới
        animator.SetBool("Atk9", false);
        animator.SetBool("Atk10", false);
        animator.SetBool("Atk11", false);
        animator.SetBool("Atk12", false);

        switch (comboStep)
        {
            case 0:
                animator.SetBool("Atk9", true);
                comboStep = 1;
                break;
            case 1:
                animator.SetBool("Atk10", true);
                comboStep = 2;
                break;
            case 2:
                animator.SetBool("Atk11", true);
                comboStep = 3;
                break;
            case 3:
                animator.SetBool("Atk12", true);
                comboStep = 4;
                break;
            default:
                ResetCombo();
                break;
        }
    }

    void ResetCombo()
    {
        comboStep = 0;
        animator.SetBool("Atk9", false);
        animator.SetBool("Atk10", false);
        animator.SetBool("Atk11", false);
        animator.SetBool("Atk12", false);
    }
}
