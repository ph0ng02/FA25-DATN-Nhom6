using UnityEngine;

public class AttackState_Dragon : StateMachineBehaviour
{
    Transform player;
    public GameObject fireEffectPrefab;   // Prefab hiệu ứng lửa
    public float yOffset = 0f;            // Độ cao spawn so với chân player
    public float effectDuration = 3f;     // Thời gian tồn tại hiệu ứng
    public float attackCooldown = 7f;     // ⬅️ 7 giây giữa mỗi lần Attack

    // Dùng biến static để đảm bảo cooldown áp dụng cho tất cả state Attack của con rồng này
    private static float lastAttackTime = -999f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ⬅️ Kiểm tra cooldown
        if (Time.time - lastAttackTime < attackCooldown)
        {
            // Chưa hết cooldown → hủy Attack
            animator.SetBool("IsAttacking", false);
            return;
        }

        // ✅ Cooldown đã hết → cho phép tấn công
        lastAttackTime = Time.time;

        // Tìm Player
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        // Spawn hiệu ứng lửa dưới chân Player
        if (player != null && fireEffectPrefab != null)
        {
            Vector3 spawnPos = new Vector3(player.position.x, player.position.y + yOffset, player.position.z);
            GameObject fire = Object.Instantiate(fireEffectPrefab, spawnPos, Quaternion.identity);

            // Tự hủy hiệu ứng sau effectDuration
            Object.Destroy(fire, effectDuration);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Quay mặt về phía Player và hủy Attack nếu Player quá xa
        if (player != null)
        {
            animator.transform.LookAt(player);
            if (Vector3.Distance(player.position, animator.transform.position) > 10f)
                animator.SetBool("IsAttacking", false);
        }
    }
}
