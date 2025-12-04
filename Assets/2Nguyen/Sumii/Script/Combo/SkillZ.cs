using UnityEngine;

public class SkillZ : MonoBehaviour
{
    [Header("Shield Prefabs")]
    public GameObject shieldPrefab;          // VFX khiên
    public GameObject breakEffectPrefab;     // VFX nổ khi bị phá
    public GameObject expireEffectPrefab;    // VFX nổ khi hết thời gian

    [Header("Settings")]
    public Transform attachPoint;            // nơi gắn khiên (Player)
    public float duration = 5f;              // thời gian tồn tại
    public float cooldown = 8f;              // hồi chiêu
    public float shieldMaxHP = 100f;         // máu khiên
    public float damageReduction = 0.5f;     // giảm 50% sát thương

    private GameObject currentShield;
    private float shieldHP;
    private float nextCastTime = 0f;
    private bool isActive = false;
    private float timer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ActivateShield();
        }

        if (isActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                ExpireShield();
            }
        }
    }

    public void ActivateShield()
    {
        if (Time.time < nextCastTime) return;
        if (isActive) return;

        // khởi tạo khiên
        currentShield = Instantiate(shieldPrefab, attachPoint.position, attachPoint.rotation);
        currentShield.transform.SetParent(attachPoint);

        shieldHP = shieldMaxHP;
        timer = duration;
        isActive = true;

        nextCastTime = Time.time + cooldown;
    }

    // hàm player gọi khi nhận damage
    public float AbsorbDamage(float incomingDamage)
    {
        if (!isActive) return incomingDamage;

        float reducedDamage = incomingDamage * (1f - damageReduction);

        shieldHP -= reducedDamage;

        if (shieldHP <= 0)
        {
            BreakShield();
        }

        return incomingDamage - reducedDamage; // phần damage còn lại Player nhận
    }

    void BreakShield()
    {
        if (breakEffectPrefab)
            Instantiate(breakEffectPrefab, attachPoint.position, Quaternion.identity);

        DisableShield();
    }

    void ExpireShield()
    {
        if (expireEffectPrefab)
            Instantiate(expireEffectPrefab, attachPoint.position, Quaternion.identity);

        DisableShield();
    }

    void DisableShield()
    {
        if (currentShield)
            Destroy(currentShield);

        isActive = false;
    }
}
