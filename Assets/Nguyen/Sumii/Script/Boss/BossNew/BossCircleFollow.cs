using UnityEngine;

public class BossCircleFollow : MonoBehaviour
{
    public Transform boss;     // boss cần bám theo
    public float offsetY = 0.05f;  // cao 0.05 trên mặt đất
    public float rotateSpeed = 60f; // tốc độ xoay

    void LateUpdate()
    {
        if (boss == null) return;

        // Luôn ở dưới chân boss
        transform.position = new Vector3(
            boss.position.x,
            boss.position.y + offsetY,
            boss.position.z
        );

        // Xoay vòng tròn
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
