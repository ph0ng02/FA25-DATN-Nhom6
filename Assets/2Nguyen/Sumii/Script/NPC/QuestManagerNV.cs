using UnityEngine;
using TMPro;

// Đảm bảo tên class là QuestManagerNV
public class QuestManagerNV : MonoBehaviour
{
    // Cần gán các đối tượng UI này trong Inspector
    public GameObject questLogPanel;
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questObjectiveText;

    // Thay Quest bằng QuestNV
    public QuestNV currentQuest;

    void Start()
    {
        if (questLogPanel != null)
        {
            questLogPanel.SetActive(false);
        }
    }

    // ⭐️ Sửa kiểu tham số thành QuestNV ⭐️
    public void StartQuestFromNPC(QuestNV newQuest)
    {
        if (newQuest == null) return;

        currentQuest = newQuest;
        UpdateQuestLogUI(currentQuest);
        Debug.Log("Nhiệm vụ mới đã được nhận: " + newQuest.questName);
    }

    public void UpdateQuestLogUI(QuestNV quest) // Sửa kiểu tham số
    {
        if (quest != null && questLogPanel != null)
        {
            questLogPanel.SetActive(true);

            if (questTitleText != null)
            {
                questTitleText.text = "Quest: " + quest.questName;
            }
            if (questObjectiveText != null)
            {
                // Truy cập objectiveDescription đã được sửa trong QuestNV.cs
                questObjectiveText.text = "Nhiệm vụ:\n\"" + quest.objectiveDescription + "\"";
            }
        }
    }
}