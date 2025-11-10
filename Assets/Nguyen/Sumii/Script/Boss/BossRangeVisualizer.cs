using UnityEngine;

[ExecuteAlways] // Cho phép hiển thị cả khi không chạy game
public class BossRangeVisualizer : MonoBehaviour
{
    [Header("Tầm hoạt động của Boss")]
    public float attackRange = 2f;
    public float throwRange = 6f;
    public float skillRange = 10f;
    public float detectionRange = 15f; // tầm phát hiện player
    public float viewAngle = 60f;      // góc nhìn của boss

    void OnDrawGizmosSelected()
    {
        // 🟥 Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 🟨 Throw range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, throwRange);

        // 🟦 Skill range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, skillRange);

        // 🟢 Detection range
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // ➡️ View angle lines
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * detectionRange);
    }
}
