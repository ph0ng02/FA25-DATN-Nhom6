using UnityEngine;

// Enum để định nghĩa các trạng thái của nhiệm vụ
public enum QuestState
{
    // 0: Chưa bắt đầu
    Inactive,
    // 1: Đã nhận, đang tìm item
    Active_FindItem,
    // 2: Đã có item, cần gặp lại Nira
    HasItem_ReturnToNira,
    // 3: Đã hoàn thành
    Completed
}

public class QuestManagerr : MonoBehaviour
{
    // Dùng static để dễ dàng truy cập từ bất kỳ script nào
    public static QuestState NiraQuestState = QuestState.Inactive;

    // Tùy chọn: Dùng event để thông báo khi trạng thái thay đổi
    public delegate void OnQuestStateChange(QuestState newState);
    public static event OnQuestStateChange OnStateChange;

    public static void UpdateNiraQuestState(QuestState newState)
    {
        NiraQuestState = newState;
        // Kích hoạt event để các UI/NPC khác cập nhật
        OnStateChange?.Invoke(NiraQuestState);
        Debug.Log($"Trạng thái Nhiệm vụ Nira được cập nhật: {newState}");
    }
}