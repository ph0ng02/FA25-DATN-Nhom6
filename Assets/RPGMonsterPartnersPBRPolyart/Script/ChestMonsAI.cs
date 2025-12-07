using UnityEngine;
using System.Collections;

public class ChestMonsterAI : MonoBehaviour
{
    private ChestMons animController;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float moveDuration = 3f;
    public float idleDuration = 1f;
    
    private Vector3 currentDirection;

    void Start()
    {
        animController = GetComponent<ChestMons>();
        
        currentDirection = GetRandomDirection();

        StartCoroutine(AILoop());
    }

    IEnumerator AILoop()
    {
        while (true)
        {
            Debug.Log("Quái vật đang chạy.");
            animController.Move(true);

            float startTime = Time.time;
            while (Time.time < startTime + moveDuration)
            {
                transform.Translate(currentDirection * moveSpeed * Time.deltaTime, Space.World);
                yield return null;
            }

            Debug.Log("Quái vật đang dừng.");
            animController.Move(false);
            yield return new WaitForSeconds(idleDuration);

            currentDirection = GetRandomDirection();
        }
    }

    private Vector3 GetRandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);

        Vector3 newDirection = new Vector3(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad),
            0f,                                  
            Mathf.Sin(randomAngle * Mathf.Deg2Rad)
        ).normalized;
        
        if (newDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(newDirection);
            transform.rotation = targetRotation;
        }

        return newDirection;
    }
}