using UnityEngine;

public class PushableRock : MonoBehaviour
{
    public float pushSpeed = 4f;
    public float pushDistance = 1.5f; // đá sẽ di chuyển đúng 1 bước
    private bool isMoving = false;
    public bool isCorrectRock = true;
    public int correctGoalID = 0; // mục tiêu đúng
    public GameObject glowEffect; // hiệu ứng ánh sáng nhẹ
    


    private Vector3 targetPosition;

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, pushSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                isMoving = false;

        }
    }
    void Start()
    {
        if (!isCorrectRock)
            glowEffect.SetActive(false); // đá giả không sáng
    }

    public void Push(Vector3 direction)
    {
        if (isMoving) return;

        targetPosition = transform.position + direction.normalized * pushDistance;
        isMoving = true;
    }
    

}
