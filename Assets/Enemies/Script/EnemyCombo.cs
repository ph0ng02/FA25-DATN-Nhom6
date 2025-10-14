using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class EnemyCombo : MonoBehaviour
{
    [System.Serializable]
    public class AttackData
    {
        public string animTrigger;     // tên trigger trong Animator
        public float windUp = 0.2f;    // trước khi hitbox active
        public float activeTime = 0.3f;// hitbox active window
        public float recovery = 0.5f;  // sau hitbox đến khi next attack possible
        public float damage = 10f;
        public float range = 2f;       // debug / optional
    }

    public AttackData[] comboAttacks;
    public float comboResetTime = 1.2f; // nếu quá lâu thì reset combo
    public Transform hitboxTransform; // chỗ active hitbox
    public LayerMask playerLayer;
    public Collider hitboxCollider; // optional: reference to collider (isTrigger)
    public void EnableHitbox() { if (hitboxCollider) hitboxCollider.enabled = true; }
    public void DisableHitbox() { if (hitboxCollider) hitboxCollider.enabled = false; }

    Animator anim;
    Coroutine comboRoutine;
    float lastAttackTime = -99f;

    public bool IsInCombo { get; private set; } = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (hitboxCollider) hitboxCollider.enabled = false;
    }

    public void StartComboSequence(Transform target)
    {
        // Start new combo only if not currently in combo
        if (comboRoutine == null)
        {
            comboRoutine = StartCoroutine(ComboFlow(target));
        }
    }

    IEnumerator ComboFlow(Transform target)
    {
        IsInCombo = true;
        int index = 0;

        while (index < comboAttacks.Length)
        {
            AttackData a = comboAttacks[index];

            // Trigger windup animation
            if (!string.IsNullOrEmpty(a.animTrigger)) anim.SetTrigger(a.animTrigger);

            // Wait windUp
            yield return new WaitForSeconds(a.windUp);

            // Activate hitbox: simple sphere check + enable collider window if provided
            if (hitboxCollider)
            {
                hitboxCollider.enabled = true;
            }
            else
            {
                // sphere overlap hit detection
                Collider[] hits = Physics.OverlapSphere(hitboxTransform.position, a.range, playerLayer);
                foreach (var h in hits)
                {
                    var dmg = h.GetComponent<Damageable>();
                    if (dmg != null)
                    {
                        dmg.TakeDamage(a.damage);
                    }
                }
            }

            // If using collider window (anim event could disable earlier) wait activeTime then disable
            yield return new WaitForSeconds(a.activeTime);

            if (hitboxCollider) hitboxCollider.enabled = false;

            // after active, wait recovery
            yield return new WaitForSeconds(a.recovery);

            lastAttackTime = Time.time;
            index++;

            // Optional: allow chaining only if player still nearby/visible
            if (target != null)
            {
                float dist = Vector3.Distance(transform.position, target.position);
                if (dist > 3.5f) break; // break combo because player run away (tweak)
            }
        }

        // combo finished -> cooldown before next combo can start
        comboRoutine = null;
        IsInCombo = false;
        yield break;
    }

    // optional: cancels combo (call when stunned/knockback/die)
    public void CancelCombo()
    {
        if (comboRoutine != null) StopCoroutine(comboRoutine);
        comboRoutine = null;
        IsInCombo = false;
        if (hitboxCollider) hitboxCollider.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        if (comboAttacks != null && comboAttacks.Length > 0 && hitboxTransform != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(hitboxTransform.position, comboAttacks[0].range);
        }
    }
}