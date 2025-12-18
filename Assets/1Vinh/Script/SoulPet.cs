using UnityEngine;

public class SoulPet : MonoBehaviour
{
    public Transform target;              // Player
    public Vector3 offset = new Vector3(0, 1.5f, -1f);
    public float followSpeed = 5f;

    private bool followPlayer = false;

    void Update()
    {
        if (!followPlayer || target == null) return;

        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            followSpeed * Time.deltaTime
        );

        float hover = Mathf.Sin(Time.time * 2f) * 0.2f;
        desiredPos.y += hover;

        // Xoay nhẹ cho đẹp
        transform.LookAt(target);
    }

    public void StartFollow(Transform player)
    {
        target = player;
        followPlayer = true;
    }
}
