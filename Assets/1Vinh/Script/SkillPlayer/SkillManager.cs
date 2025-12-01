using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public bool hasCircleSlash = false;

    private void Awake()
    {
        Instance = this;
    }

    public void UnlockCircleSlash()
    {
        hasCircleSlash = true;
        Debug.Log("Đã mở skill Circle Slash!");
    }
}
