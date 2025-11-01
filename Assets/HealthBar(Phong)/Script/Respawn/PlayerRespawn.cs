using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 deathPosition;
    private PlayerHealth playerHealth;
    public float respawnDelay = 2f; // thời gian chờ hồi sinh

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void Die()
    {
        // Lưu vị trí chết
        deathPosition = transform.position;

        // Ẩn nhân vật hoặc tạm thời disable collider/renderer
        GetComponent<MeshRenderer>().enabled = false;

        // Gọi hàm Respawn sau vài giây
        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Di chuyển lại đúng chỗ cũ
        transform.position = deathPosition;

        // Hồi đầy máu
        playerHealth.currentHealth = playerHealth.maxHealth;
        if (playerHealth.healthSlider != null)
            playerHealth.healthSlider.value = playerHealth.maxHealth;

        // Cập nhật lại tim và text
        var heartsMethod = playerHealth.GetType().GetMethod("Heal");
        if (heartsMethod != null)
            playerHealth.Heal(0); // cập nhật UI

        // Hiện lại nhân vật
        GetComponent<MeshRenderer>().enabled = true;

        // Mở lại thời gian game (nếu bạn có dừng)
        Time.timeScale = 1f;

        // Ẩn UI game over
        if (playerHealth.gameOverUI != null)
            playerHealth.gameOverUI.SetActive(false);

        Debug.Log("Player đã respawn tại " + transform.position);
    }
}
