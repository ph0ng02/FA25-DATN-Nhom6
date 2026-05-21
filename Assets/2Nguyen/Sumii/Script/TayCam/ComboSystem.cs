using UnityEngine;
using UnityEngine.InputSystem;

public class ComboSystem : MonoBehaviour
{
    private Animator animator;
    private InputSystem_Actions inputActions;

    private int comboStep = 0;
    private float lastInputTime = 0f;
    private float comboResetDelay = 1f;
    private float comboCooldown = 0.3f; // delay giữa các đòn combo

    private string currentSkill = "";
    private bool isAttacking = false;
    private bool canNextCombo = true;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!canNextCombo) return;

        if (inputActions.Player.SkillQ.triggered)
            HandleCombo("Q");

        if (inputActions.Player.SkillE.triggered)
            HandleCombo("E");

        if (inputActions.Player.SkillR.triggered)
            HandleCombo("R");

        if (isAttacking && Time.time - lastInputTime > comboResetDelay)
            ResetCombo();
    }

    void HandleCombo(string skill)
    {
        // Nếu đổi skill giữa chừng thì reset combo
        if (currentSkill != skill)
        {
            ResetCombo();
            currentSkill = skill;
        }

        lastInputTime = Time.time;
        isAttacking = true;
        comboStep++;

        int maxCombo = skill switch
        {
            "Q" => 4,
            "E" => 3,
            "R" => 2,
            _ => 1
        };

        if (comboStep > maxCombo)
        {
            ResetCombo();
            return;
        }

        // Reset tất cả trigger cũ
        for (int i = 9; i <= 20; i++)
            animator.ResetTrigger("Atk" + i);

        // Xác định animation tương ứng
        int animIndex = skill switch
        {
            "Q" => 8 + comboStep,  // 9–12
            "E" => 12 + comboStep, // 13–16
            "R" => 16 + comboStep, // 17–18
            _ => 9
        };

        // Gọi animation combo
        animator.SetTrigger("Atk" + animIndex);

        // Tạm khóa combo tiếp theo trong thời gian cooldown
        StartCoroutine(ComboDelay());
    }

    System.Collections.IEnumerator ComboDelay()
    {
        canNextCombo = false;
        yield return new WaitForSeconds(comboCooldown);
        canNextCombo = true;
    }

    void ResetCombo()
    {
        isAttacking = false;
        comboStep = 0;
        currentSkill = "";
        canNextCombo = true;

        for (int i = 9; i <= 20; i++)
            animator.ResetTrigger("Atk" + i);

        animator.CrossFade("Idle", 0.1f);
    }
}
