// Trong script QuestManager.cs

// Hàm này BẮT BUỘC phải là PUBLIC
public void StartQuestFromNPC(Quest newQuest)
{
    // Kiểm tra tính hợp lệ của nhiệm vụ
    if (newQuest == null) return;

    // Gán nhiệm vụ hiện tại (ví dụ)
    // currentQuest = newQuest;

    // Cập nhật giao diện
    // UpdateQuestLogUI(newQuest); 

    Debug.Log("Nhiệm vụ mới đã được nhận: " + newQuest.questName);
}