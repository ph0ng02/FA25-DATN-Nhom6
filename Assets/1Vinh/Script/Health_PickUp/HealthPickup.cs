using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 20;    // Lượng máu hồi khi nhặt
    public float rotateSpeed = 60f;

    private void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra player có HealthManagement không
        HealthManagement health = other.GetComponent<HealthManagement>();

        if (health != null)
        {
            health.Heal(healAmount);      // GỌI HÀM HEAL TRONG SCRIPT CỦA BẠN
            Debug.Log("❤️ Player hồi: " + healAmount);

            Destroy(gameObject);          // Xóa vật phẩm máu sau khi nhặt
        }
    }
}
