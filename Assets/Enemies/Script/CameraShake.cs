using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Hàm chính để rung camera
    public IEnumerator Shake(float duration = 0.2f, float magnitude = 0.3f)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    // ✅ Thêm hàm này để tương thích với Enemy.cs và HealthSystem.cs
    public void ShakeCamera(float duration = 0.2f, float magnitude = 0.3f)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    // Hoặc nếu bạn muốn gọi đơn giản không có tham số:
    public void ShakeCamera()
    {
        StartCoroutine(Shake(0.2f, 0.3f));
    }
}
