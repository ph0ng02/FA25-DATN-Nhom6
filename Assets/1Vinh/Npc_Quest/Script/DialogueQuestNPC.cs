using UnityEngine;
using TMPro;

public class DialogueQuestNPC : MonoBehaviour
{
    [Header("Dialogue")]
    [TextArea(3, 10)]
    public string[] dialogueLines;
    public string[] speakerLines;
    private int dialogueIndex = 0;

    [Header("Quest")]
    public Quest quest;

    [Header("UI")]
    public GameObject uiPanel;
    public TextMeshProUGUI uiText;
    public TextMeshProUGUI speakerText;

    private bool playerInside = false;
    private bool isTalking = false;

    private bool hasTalked = false;   // <--- THÊM DÒNG NÀY

    void Start()
    {
        uiPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            uiPanel.SetActive(true);

            if (!hasTalked)
                uiText.text = "Nhấn E để trò chuyện";
            else
                uiText.text = "Nhấn E để xem nhiệm vụ";  // <--- LẦN SAU KHÔNG THOẠI
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            isTalking = false;
            dialogueIndex = 0;
            uiPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            // Nếu đã trò chuyện rồi -> vào thẳng phần nhiệm vụ
            if (hasTalked)
            {
                HandleQuest();
                return;
            }

            // Chưa nói -> hiển thị thoại lần đầu
            if (!isTalking)
                StartDialogue();
            else
                NextDialogue();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        dialogueIndex = 0;
        ShowDialogue();
    }

    void NextDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex < dialogueLines.Length)
        {
            ShowDialogue();
        }
        else
        {
            // kết thúc thoại
            hasTalked = true;   // <--- ĐÁNH DẤU LÀ ĐÃ THOẠI 1 LẦN
            HandleQuest();
        }
    }

    void ShowDialogue()
    {
        uiText.text = dialogueLines[dialogueIndex];
        speakerText.text = speakerLines[dialogueIndex];
    }

    void HandleQuest()
    {
        speakerText.text = "NPC";

        // 1. Chưa nhận nhiệm vụ
        if (!quest.isAccepted)
        {
            uiText.text =
                $"Nhiệm vụ: {quest.questName}\n\n{quest.description}\n\nNhấn E để nhận nhiệm vụ";

            quest.isAccepted = true;
            QuestManager.Instance.currentQuest = quest;
            return;
        }

        // 2. Đang làm
        if (!quest.isCompleted)
        {
            uiText.text =
                $"Tiến độ: {quest.currentKillCount}/{quest.requiredKillCount}\nHãy đi tiêu diệt đủ quái!";
            return;
        }

        // 3. Hoàn thành nhiệm vụ
        if (quest.isCompleted)
        {
            uiText.text =
                $"🎉 Bạn đã hoàn thành nhiệm vụ!\nĐã mở khóa skill Circle Slash!";

            SkillManager.Instance.UnlockCircleSlash();
            return;
        }
    }
}
