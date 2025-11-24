using UnityEngine;

// Cho phép tạo Asset Quest mới qua Menu
[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
    // Cần KHỚP CHÍNH XÁC với QuestManagerNV.cs
    public string questName = "Tên nhiệm vụ mặc định";

    // ⭐️ ĐÂY LÀ PHẦN KHẮC PHỤC LỖI CS1061 ⭐️
    [TextArea(3, 10)]
    public string objectiveDescription = "Mục tiêu: Giết quái vật.";

    public bool isCompleted = false;

    // Các trường dữ liệu khác
    // public int currentProgress;
    // public int requiredAmount;
}