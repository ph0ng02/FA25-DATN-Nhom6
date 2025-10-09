using UnityEngine;

public class DoorSystemManager : MonoBehaviour
{
    [Header("References")]
    public LeverSwitch leverA;
    public LeverSwitch leverB;
    public DoorController doorController;

    public void CheckLevers()
    {
        if (leverA.isActivated && leverB.isActivated)
        {
            doorController.isOpen = true;
            Debug.Log("✅ Cả hai cần gạt bật - cửa mở!");
        }
        else
        {
            doorController.isOpen = false;
            Debug.Log("❌ Chưa đủ hai cần gạt - cửa đóng!");
        }
    }
}
