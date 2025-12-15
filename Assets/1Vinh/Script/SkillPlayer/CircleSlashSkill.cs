using UnityEngine;

public class CircleSlashSkill : MonoBehaviour
{
    [Header("VFX")]
    public GameObject slashVFX;   // Prefab skill
    public Transform spawnPoint;  // Vị trí tạo skill (thường là chân nhân vật)
    public float vfxLifetime = 1f; // thời gian tồn tại VFX

    [Header("Settings")]
    public float cooldown = 5f;
    private float lastTimeUsed = 2f;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Nhấn F để dùng skill nếu mở khóa
        if (Input.GetKeyDown(KeyCode.F) && SkillManager.Instance.hasCircleSlash)
        {
            TryUseSkill();
        }
    }

    void TryUseSkill()
    {
        if (Time.time - lastTimeUsed < cooldown)
            return;

        // Play animation
        anim.SetTrigger("CircleSlash");

        lastTimeUsed = Time.time;
    }

    // Gọi từ Animation Event
    public void SpawnSlashVFX()
    {
        if (slashVFX != null && spawnPoint != null)
        {
            GameObject vfx = Instantiate(slashVFX, spawnPoint.position, spawnPoint.rotation);

            // AUTO DESTROY VFX SAU 2 GIÂY
            Destroy(vfx, vfxLifetime);
        }
    }
}
