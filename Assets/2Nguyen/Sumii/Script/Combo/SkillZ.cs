using UnityEngine;

public class SkillZ : MonoBehaviour
{
    [Header("Shield Prefab")]
    public GameObject shieldPrefab;

    [Header("Settings")]
    public Transform attachPoint;
    public float duration = 5f;
    public float cooldown = 8f;
    public float shieldHP = 50f; // máu khiên
    public float damageReduction = 0.5f;

    private GameObject currentShield;
    private float currentHP;
    private float timer;
    private float nextCastTime;
    private bool isActive;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            ActivateShield();

        if (isActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                RemoveShield();
        }
    }

    // Gọi khi nhấn Z
    public void ActivateShield()
    {
        if (Time.time < nextCastTime) return;

        RemoveShield(); // luôn xóa sạch shield cũ trước

        currentShield = Instantiate(shieldPrefab, attachPoint);
        currentShield.transform.localPosition = Vector3.zero;
        currentShield.transform.localRotation = Quaternion.identity;

        currentHP = shieldHP;
        timer = duration;
        isActive = true;

        nextCastTime = Time.time + cooldown;
    }

    // Gọi từ Player khi nhận damage
    public float AbsorbDamage(float incomingDamage)
    {
        if (!isActive) return incomingDamage;

        float reduced = incomingDamage * (1f - damageReduction);
        currentHP -= reduced;

        if (currentHP <= 0)
            RemoveShield();

        return incomingDamage - reduced; // damage player nhận
    }

    // Bỏ khiên
    private void RemoveShield()
    {
        if (currentShield != null)
        {
            Destroy(currentShield);
            currentShield = null;
        }

        isActive = false;
    }
}
