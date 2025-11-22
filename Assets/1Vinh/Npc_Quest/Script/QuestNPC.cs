using UnityEngine;

public class QuestNPC : MonoBehaviour
{
    public Quest quest;          // nhiệm vụ mà NPC này giao
    public GameObject uiPanel;   // panel UI
    public TMPro.TextMeshProUGUI uiText;

    private bool playerInside = false;

    void Start()
    {
        uiPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            uiPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        uiPanel.SetActive(true);

        // 1. Chưa nhận nhiệm vụ
        if (!quest.isAccepted)
        {
            uiText.text =
                $"Nhiệm vụ: {quest.questName}\n\n{quest.description}\n\nNhấn E để nhận nhiệm vụ";

            quest.isAccepted = true;
            QuestManager.Instance.currentQuest = quest;
            return;
        }

        // 2. Đang làm nhiệm vụ nhưng chưa đủ kill
        if (quest.isAccepted && !quest.isCompleted)
        {
            uiText.text =
                $"Bạn đang làm nhiệm vụ...\n" +
                $"Tiến độ: {quest.currentKillCount}/{quest.requiredKillCount}\n\n" +
                $"Hãy tiêu diệt đủ quái!";
            return;
        }

        // 3. Hoàn thành nhiệm vụ
        if (quest.isCompleted)
        {
            uiText.text =
                $"🎉 Hoàn thành nhiệm vụ!\nBạn đã tiêu diệt đủ quái.\n\nNhấn E để nhận thưởng";

            // Ở đây có thể thêm thưởng tiền, exp, mở cửa, trigger sự kiện...
            // Ví dụ mở cửa:
            // door.Open();

            return;
        }
    }
}
