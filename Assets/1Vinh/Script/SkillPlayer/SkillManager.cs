using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public bool hasCircleSlash = false; // BAN ĐẦU BỊ KHÓA

    private void Awake()
    {
        Instance = this;
        Debug.Log("SkillManager Awake chạy rồi!");
    }

    public void UnlockCircleSlash()
    {
        hasCircleSlash = true;
        Debug.Log("Đã mở skill Circle Slash!");
    }
}
