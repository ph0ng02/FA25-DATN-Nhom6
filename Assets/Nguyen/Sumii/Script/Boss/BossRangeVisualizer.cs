using UnityEngine;

[ExecuteAlways]
public class BossRangeVisualizer : MonoBehaviour
{
    [Header("📏 Tầm hoạt động của Boss")]
    public float attackRange = 2f;
    public float throwRange = 6f;
    public float skillRange = 10f;
    public float detectionRange = 15f; // tầm phát hiện player
    public float viewAngle = 60f;      // góc nhìn của boss

    [Header("⚡ Skill Beam")]
    public float beamRange = 12f;      // tầm bắn của beam
    public float beamAngle = 15f;      // góc lan của tia beam (nhỏ hơn viewAngle)
    public Color beamColor = new Color(1f, 0.3f, 0f, 0.35f); // cam nhạt

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;

        // 🟥 Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos, attackRange);

        // 🟨 Throw range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pos, throwRange);

        // 🟦 Skill range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(pos, skillRange);

        // 🟢 Detection range
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(pos, detectionRange);

        // ➡️ View angle
        Vector3 leftView = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 rightView = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos, pos + leftView * detectionRange);
        Gizmos.DrawLine(pos, pos + rightView * detectionRange);

        // ⚡ Beam Skill Cone
        DrawBeamRange(pos);
    }

    void DrawBeamRange(Vector3 origin)
    {
        Gizmos.color = beamColor;

        // Vẽ hướng chính giữa của beam
        Vector3 forward = transform.forward;

        // Tính biên trái & phải của góc beam
        Vector3 leftDir = Quaternion.Euler(0, -beamAngle / 2f, 0) * forward;
        Vector3 rightDir = Quaternion.Euler(0, beamAngle / 2f, 0) * forward;

        // Vẽ các đường giới hạn beam
        Gizmos.DrawLine(origin, origin + leftDir * beamRange);
        Gizmos.DrawLine(origin, origin + rightDir * beamRange);

        // Vẽ mặt cong giả lập chóp tia beam
        int segments = 16;
        Vector3 prevPoint = origin + leftDir * beamRange;
        for (int i = 1; i <= segments; i++)
        {
            float angle = -beamAngle / 2f + (beamAngle / segments) * i;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * forward;
            Vector3 point = origin + dir * beamRange;
            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }
    }
}
