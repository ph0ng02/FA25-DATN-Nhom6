using UnityEngine;
using TMPro;

public class QuestManagerNV : MonoBehaviour
{
    // Cần gán các đối tượng UI này trong Inspector
    public GameObject questLogPanel;
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questObjectiveText;

    public Quest currentQuest; // Nhiệm vụ hiện tại

    void Start()
    {
        // Ẩn log nhiệm vụ khi bắt đầu nếu không có nhiệm vụ
        if (questLogPanel != null)
        {
            questLogPanel.SetActive(false);
        }
    }

    // ⭐️ HÀM QUAN TRỌNG: Được gọi bởi NPCDialogueNV2 ⭐️
    public void StartQuestFromNPC(Quest newQuest)
    {
        if (newQuest == null) return;

        currentQuest = newQuest;

        // Cập nhật giao diện Quest Log
        UpdateQuestLogUI(currentQuest);

        Debug.Log("Nhiệm vụ mới đã được nhận: " );
    }

    public void UpdateQuestLogUI(Quest quest)
    {
        if (quest != null && questLogPanel != null)
        {
            // Hiển thị Panel
            questLogPanel.SetActive(true);

            // Cập nhật Text
            if (questTitleText != null)
            {
                questTitleText.text = "Quest: " ;
            }
            if (questObjectiveText != null)
            {
                // Sử dụng định dạng mong muốn: "Nhiệm vụ:\n"
                questObjectiveText.text = "Nhiệm vụ:\n\"" + quest.objectiveDescription + "\"";
            }
        }
    }
}