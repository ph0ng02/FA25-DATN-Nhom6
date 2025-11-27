using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public bool circleSlashUnlocked = false;

    private void Awake()
    {
        Instance = this;
    }

    public void UnlockCircleSlash()
    {
        circleSlashUnlocked = true;
        Debug.Log("Đã học skill Circle Slash!");
    }
}