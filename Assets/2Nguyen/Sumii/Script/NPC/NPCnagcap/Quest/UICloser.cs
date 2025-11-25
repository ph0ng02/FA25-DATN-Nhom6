using UnityEngine;

public class UICloser : MonoBehaviour
{
    [Header("Panels cần đóng")]
    public GameObject shopPanel;
    public GameObject inventoryPanel;

    public void CloseAll()
    {
        if (shopPanel != null) shopPanel.SetActive(false);
        if (inventoryPanel != null) inventoryPanel.SetActive(false);

        // Khóa chuột lại như khi chơi
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}