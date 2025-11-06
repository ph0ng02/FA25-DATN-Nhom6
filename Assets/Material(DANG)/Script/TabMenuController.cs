using UnityEngine;
using UnityEngine.UI;

public class TabMenuController : MonoBehaviour
{
    [Header("Main Panels")]
    public GameObject panelMain;
    public GameObject panelLog;
    public GameObject panelSetting;
    public GameObject panelTabs;

    [Header("Tab Buttons")]
    public Button buttonLog;
    public Button buttonSetting;

    private bool isOpen = false;

    void Start()
    {
        panelMain.SetActive(false);
        panelLog.SetActive(false);
        panelSetting.SetActive(false);
        panelTabs.SetActive(false);

        buttonLog.onClick.AddListener(ShowLogTab);
        buttonSetting.onClick.AddListener(ShowSettingTab);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            panelMain.SetActive(isOpen);
            panelTabs.SetActive(isOpen);

            if (isOpen)
            {
                // Khi mở bảng → hiện chuột + thả tự do
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                ShowLogTab(); // Mặc định tab log bật lên
            }
            else
            {
                // Khi tắt bảng → ẩn chuột + khóa lại cho camera
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void ShowLogTab()
    {
        panelLog.SetActive(true);
        panelSetting.SetActive(false);
    }

    void ShowSettingTab()
    {
        panelLog.SetActive(false);
        panelSetting.SetActive(true);
    }
}
