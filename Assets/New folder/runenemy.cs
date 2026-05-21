using UnityEngine;

public class runenemy : MonoBehaviour
{
      public float moveSpeed = 1.5f;      // tốc độ chậm
    public float changeDirTime = 3f;    // đổi hướng mỗi 3 giây
    float timer = 0f;
    Vector3 direction;

    void Start()
    {
        PickNewDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // sau mỗi khoảng thời gian -> đổi hướng
        if (timer >= changeDirTime)
        {
            PickNewDirection();
        }

        // di chuyển theo hướng đã chọn
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        // quay mặt về hướng di chuyển
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 2f);
        }
    }

    void PickNewDirection()
    {
        timer = 0f;

        // hướng ngẫu nhiên trên mặt đất
        direction = new Vector3(
            Random.Range(-1f, 1f),
            0,
            Random.Range(-1f, 1f)
        ).normalized;
    }
}
