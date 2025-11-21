using UnityEngine;

public class AttackControl : MonoBehaviour
{
    private Animator anim;
    private int comboStep = 0;
    private float comboTimer = 0f;
    private float comboWindow = 0.6f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Combo timer
        if (comboTimer > 0)
            comboTimer -= Time.deltaTime;
        else
            ResetCombo();

        // Left click combo
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            DoCombo();
        }

        // Forward Attack (Y key)
        if (Input.GetKeyDown(KeyCode.Y))
        {
            DoForwardAttack();
        }
    }

    void DoCombo()
    {
        comboStep++;

        if (comboStep > 4)
            comboStep = 1;

        anim.SetInteger("Combo", comboStep);
        anim.SetBool("Attack", true);

        comboTimer = comboWindow;
    }

    void DoForwardAttack()
    {
        // Reset combo để không bị lẫn
        ResetCombo();

        anim.SetBool("Forward", true);   // bật Forward Attack
    }

    // Gọi trong cuối animation Slash1, Slash2...
    public void EndAttack()
    {
        anim.SetBool("Attack", false);
    }

    // Gọi trong cuối animation Forward Attack
    public void EndForward()
    {
        anim.SetBool("Forward", false);
    }

    void ResetCombo()
    {
        comboStep = 0;
        anim.SetInteger("Combo", 0);
        anim.SetBool("Attack", false);
    }
}