using UnityEngine;

public class EnemyCaster : MonoBehaviour
{
    public Transform player;
    public Animator animator;

    [Header("Skill Settings")]
    public GameObject projectilePrefab;
    public Transform castPoint;

    public float detectRange = 10f;
    public float castCooldown = 2.5f;
    public float castAnimationDuration = 1.0f;
    public float projectileSpawnTime = 0.5f; // thời điểm tạo projectile (0.5s)

    private float lastCastTime = 0f;
    private bool isCasting = false;
    private float castTimer = 0f;

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // Nếu enemy đang cast → đếm thời gian spawn projectile
        if (isCasting)
        {
            castTimer += Time.deltaTime;

            // Đến đúng khung hình spawn projectile
            if (castTimer >= projectileSpawnTime)
            {
                SpawnProjectile();
                projectileSpawnTime = 999f; // tránh bắn nhiều lần
            }

            // Kết thúc animation cast
            if (castTimer >= castAnimationDuration)
            {
                isCasting = false;
            }

            return;
        }

        // Nếu player trong range và cooldown đủ → cast skill
        if (dist <= detectRange && Time.time - lastCastTime >= castCooldown)
        {
            StartCast();
        }
    }

    void StartCast()
    {
        isCasting = true;
        castTimer = 0f;
        projectileSpawnTime = 0.5f; // reset lại
        lastCastTime = Time.time;

        LookAtPlayer();
        animator.SetTrigger("CastSpell");
    }

    void SpawnProjectile()
    {
        GameObject obj = Instantiate(projectilePrefab, castPoint.position, castPoint.rotation);

        // Nếu projectile có script tự bay thì không cần set gì thêm
        var proj = obj.GetComponent<MagicProjectile>();
        if (proj != null) proj.SetTarget(player);
    }

    void LookAtPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
