using UnityEngine;
using FullOpaqueVFX;     // ★ Thêm dòng này để Unity tìm thấy SpellData

public class SkillX : MonoBehaviour
{
    public SpellData spellData;         // ★ Dùng class SpellData
    public Transform castPoint;         // nơi spawn hiệu ứng
    private bool isCasting = false;
    private float cooldownTimer = 0f;

    void Update()
    {
        // Cooldown update
        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;

        // Nếu bấm phím kích hoạt
        if (Input.GetKeyDown(spellData.activationKey))
        {
            TryCastSkill();
        }
    }

    void TryCastSkill()
    {
        if (cooldownTimer > 0 || isCasting)
            return;

        StartCoroutine(CastRoutine());
    }

    System.Collections.IEnumerator CastRoutine()
    {
        isCasting = true;

        // 1. Hiệu ứng charge (incantation)
        if (spellData.incantationPrefab != null)
        {
            spellData.SpawnEffect(
                spellData.incantationPrefab,
                castPoint.position,
                castPoint.rotation
            );
        }

        // 2. Thời gian cast
        yield return new WaitForSeconds(spellData.castTime);

        // 3. Hiệu ứng burst
        if (spellData.spellBurstPrefab != null)
        {
            spellData.SpawnEffect(
                spellData.spellBurstPrefab,
                castPoint.position,
                castPoint.rotation
            );
        }

        // 4. Main spell (đạn hoặc vụ nổ)
        if (spellData.mainSpellPrefab != null)
        {
            SpawnMainSpell();
        }

        // Reset cooldown
        cooldownTimer = spellData.cooldown;
        isCasting = false;
    }

    void SpawnMainSpell()
    {
        Vector3 spawnPos = castPoint.position;
        Quaternion spawnRot = castPoint.rotation;

        switch (spellData.spellTargetBehavior)
        {
            case SpellData.SpellTargetBehavior.SpawnOnCaster:
                spawnPos = castPoint.position;
                break;

            case SpellData.SpellTargetBehavior.SpawnOnTarget:
                spawnPos = GetTargetPosition();
                break;

            case SpellData.SpellTargetBehavior.FromCasterLookAtTarget:
                Vector3 target = GetTargetPosition();
                spawnRot = Quaternion.LookRotation(target - spawnPos);
                break;
        }

        spellData.SpawnEffect(spellData.mainSpellPrefab, spawnPos, spawnRot);
    }

    Vector3 GetTargetPosition()
    {
        // Raycast theo hướng camera
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
            return hit.point;

        return castPoint.position + castPoint.forward * 5f;
    }
}
