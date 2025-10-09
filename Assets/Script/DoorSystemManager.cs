using UnityEngine;

public class DoorSystemManager : MonoBehaviour
{
    [Header("References")]
    public LeverSwitch leverA;
    public LeverSwitch leverB;

    public DoorController doorA;
    public DoorController doorB;

    public void CheckLevers()
    {
        if (leverA.isActivated && leverB.isActivated)
        {
            doorA.isOpen = true;
            doorB.isOpen = true;
            Debug.Log(" Cả hai cần gạt bật - Hai cửa mở!");
        }
        else
        {
            doorA.isOpen = false;
            doorB.isOpen = false;
            Debug.Log(" Chưa đủ hai cần gạt - Hai cửa đóng!");
        }
    }
}
