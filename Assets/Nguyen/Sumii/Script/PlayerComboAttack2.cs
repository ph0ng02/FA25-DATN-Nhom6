using UnityEngine;

public class PlayerComboAttack2 : MonoBehaviour
{
    [Header("Animation Settings")]
    public Animator animator;              // Kéo Animator vào đây
    public string layerName = "Attack2";   // Tên layer chứa Atk5–8
    public float comboResetTime = 1.0f;    // Thời gian reset combo

    private int comboStep = 0;
    private float comboTimer = 0f;
    private int layerIndex = -1;

    void Start()
    {
        // Lấy Animator nếu quên gán
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        // Lấy index của layer theo tên (an toàn hơn số)
        if (animator != null)
        {
            layerIndex = animator.GetLayerIndex(layerName);
            if (layerIndex == -1)
                Debug.LogError($"❌ Không tìm thấy layer '{layerName}' trong Animator!");
            else
                Debug.Log($"✅ Layer '{layerName}' index = {layerIndex}");
        }
        else
        {
            Debug.LogError("❌ Animator chưa được gán!");
        }
    }

    void Update()
    {
        // Giảm timer, reset combo nếu chờ lâu
        if (comboTimer > 0)
            comboTimer -= Time.deltaTime;
        else
            comboStep = 0;

        // Khi nhấn Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            comboTimer = comboResetTime;
            comboStep++;
            if (comboStep > 4) comboStep = 1;

            string animName = "Atk" + (comboStep + 4); // 5–8
            Debug.Log($"🌀 Combo Step {comboStep} → Play {animName}");

            PlayAttack(animName);
        }
    }

    void PlayAttack(string stateName)
    {
        if (animator == null)
        {
            Debug.LogError("❌ Chưa có Animator!");
            return;
        }

        if (layerIndex == -1)
        {
            Debug.LogError("❌ Layer index không hợp lệ!");
            return;
        }

        Debug.Log($"▶️ Phát animation {stateName} trên layer {layerIndex}");
        animator.Play(stateName, layerIndex, 0f);
    }
}
