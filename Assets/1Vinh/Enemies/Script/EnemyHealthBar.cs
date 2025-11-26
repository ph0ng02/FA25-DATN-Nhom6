using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image fillImage;
    private Transform target;
    private Vector3 offset = new Vector3(0, 2.5f, 0);

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    public void SetHealth(float current, float max)
    {
        if (fillImage != null)
            fillImage.fillAmount = current / max;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.forward = Camera.main.transform.forward; // Luôn quay camera
        }
    }
}
